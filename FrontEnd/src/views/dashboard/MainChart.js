import React, { useEffect, useState, useRef } from 'react';
import { CButton, CButtonGroup, CCardBody } from '@coreui/react';
import { CChartLine } from '@coreui/react-chartjs';
import { getStyle } from '@coreui/utils';

const MainChart = () => {
  const chartRef = useRef(null);
  const [data, setData] = useState([]);
  const [labels, setLabels] = useState([]);
  const [activeButton, setActiveButton] = useState('Day');
  const [loading, setLoading] = useState(false);

  const fetchData = async (endpoint) => {
    setLoading(true);
    try {
      const response = await fetch(`https://localhost:44319/api/Memberships/${endpoint}`);
      const result = await response.json();
      const newLabels = result.map(item => item.Date || item.Week || item.Month || item.Year.toString());
      const newChartData = result.map(item => item.Count);
      setLabels(newLabels);
      setData(newChartData);
    } catch (error) {
      console.error('Failed to fetch data:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const now = new Date();
    const year = now.getFullYear();
    const month = now.getMonth() + 1; // Months are zero-based in JavaScript

    fetchData(`DailyCounts?year=${year}&month=${month}`); // Default data

    const updateChartStyles = () => {
      if (chartRef.current) {
        setTimeout(() => {
          const chart = chartRef.current.chartInstance;
          chart.options.scales.x.grid.borderColor = getStyle('--cui-border-color-translucent');
          chart.options.scales.x.grid.color = getStyle('--cui-border-color-translucent');
          chart.options.scales.x.ticks.color = getStyle('--cui-body-color');
          chart.options.scales.y.grid.borderColor = getStyle('--cui-border-color-translucent');
          chart.options.scales.y.grid.color = getStyle('--cui-border-color-translucent');
          chart.options.scales.y.ticks.color = getStyle('--cui-body-color');
          chart.update();
        });
      }
    };

    document.documentElement.addEventListener('ColorSchemeChange', updateChartStyles);
    return () => {
      document.documentElement.removeEventListener('ColorSchemeChange', updateChartStyles);
    };
  }, []);

  const handleButtonClick = (value) => {
    setActiveButton(value);
    const now = new Date();
    const year = now.getFullYear();
    const month = now.getMonth() + 1; // Months are zero-based in JavaScript

    if (value === 'Day') fetchData(`DailyCounts?year=${year}&month=${month}`);
    if (value === 'Week') fetchData(`WeeklyCounts?year=${year}&month=${month}`);
    if (value === 'Month') fetchData(`MonthlyCounts?year=${year}`);
    if (value === 'Year') fetchData(`YearlyCounts`);
  };

  return (
    <>
      <CButtonGroup className="float-end me-3">
        {['Day', 'Week', 'Month', 'Year'].map((value) => (
          <CButton
            color="outline-secondary"
            key={value}
            className="mx-0"
            active={value === activeButton}
            onClick={() => handleButtonClick(value)}
          >
            {value}
          </CButton>
        ))}
      </CButtonGroup>

      <CCardBody>
        {loading ? (
          <p>Loading...</p>
        ) : (
          <CChartLine
            ref={chartRef}
            style={{ height: '300px', marginTop: '40px' }}
            data={{
              labels: labels,
              datasets: [
                {
                  label: 'User Count',
                  backgroundColor: `rgba(${getStyle('--cui-info-rgb')}, .1)`,
                  borderColor: getStyle('--cui-info'),
                  pointHoverBackgroundColor: getStyle('--cui-info'),
                  borderWidth: 2,
                  data: data,
                  fill: true,
                },
              ],
            }}
            options={{
              maintainAspectRatio: false,
              plugins: {
                legend: {
                  display: false,
                },
              },
              scales: {
                x: {
                  grid: {
                    color: getStyle('--cui-border-color-translucent'),
                    drawOnChartArea: false,
                  },
                  ticks: {
                    color: getStyle('--cui-body-color'),
                    maxRotation: 45, // Adjust to fit the labels better
                    minRotation: 45,
                  },
                },
                y: {
                  beginAtZero: true,
                  border: {
                    color: getStyle('--cui-border-color-translucent'),
                  },
                  grid: {
                    color: getStyle('--cui-border-color-translucent'),
                  },
                  ticks: {
                    color: getStyle('--cui-body-color'),
                    maxTicksLimit: 10, // Increase number of ticks for better granularity
                    stepSize: Math.ceil(Math.max(...data) / 10), // Adjust step size based on the max value
                  },
                },
              },
              elements: {
                line: {
                  tension: 0.4,
                },
                point: {
                  radius: 0,
                  hitRadius: 10,
                  hoverRadius: 4,
                  hoverBorderWidth: 3,
                },
              },
            }}
          />
        )}
      </CCardBody>
    </>
  );
};

export default MainChart;
