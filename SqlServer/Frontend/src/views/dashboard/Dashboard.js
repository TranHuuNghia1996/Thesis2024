import React from 'react'

import {
  CAvatar,
  CButton,
  CButtonGroup,
  CCard,
  CCardBody,
  CCardFooter,
  CCardHeader,
  CCol,
  CProgress,
  CRow,
  CTable,
  CTableBody,
  CTableDataCell,
  CTableHead,
  CTableHeaderCell,
  CPagination ,
  CPaginationItem,
  CTableRow,
} from '@coreui/react'
import { CChartLine } from '@coreui/react-chartjs'
import { getStyle, hexToRgba } from '@coreui/utils'
import CIcon from '@coreui/icons-react'
import {
  cibCcAmex,
  cibCcApplePay,
  cibCcMastercard,
  cibCcPaypal,
  cibCcStripe,
  cibCcVisa,
  cibGoogle,
  cibFacebook,
  cibLinkedin,
  cifBr,
  cifEs,
  cifFr,
  cifIn,
  cifPl,
  cifUs,
  cibTwitter,
  cilCloudDownload,
  cilPeople,
  cilUser,
  cilUserFemale,
} from '@coreui/icons'


import { useEffect,useState } from 'react'

import avatar1 from 'src/assets/images/avatars/1.jpg'
import avatar2 from 'src/assets/images/avatars/2.jpg'
import avatar3 from 'src/assets/images/avatars/3.jpg'
import avatar4 from 'src/assets/images/avatars/4.jpg'
import avatar5 from 'src/assets/images/avatars/5.jpg'
import avatar6 from 'src/assets/images/avatars/6.jpg'

import WidgetsBrand from '../widgets/WidgetsBrand'
import WidgetsDropdown from '../widgets/WidgetsDropdown'

const Dashboard = () => {

  const [membershipCount, setMembershipCount] = useState([]); // Initialize with empty array
  const [membershipCountByCurrentWeek, setmembershipCountByCurrentWeek] = useState([]); // Initialize as an empty array 
  const [membershipCountByYearForRecentYears, setmembershipCountByYearForRecentYears] = useState([]); // Initialize as an empty array 
  const [selectedView, setSelectedView] = useState('Month'); // Initially Month


    // Function to fetch appropriate data based on selection
    const fetchData = async () => {
      try {
          let url = '';
          if (selectedView === 'Day') {
              url = 'https://localhost:44395/Users/membershipCountByCurrentWeek';
          } else if (selectedView === 'Month') {
              url = `https://localhost:44395/Users/membershipCountByMonth`; // Replace with dynamic year calculation as needed
          } else { // selectedView === 'Year'
              url = 'https://localhost:44395/Users/membershipCountByYearForRecentYears';
          }
  
          const response = await fetch(url);
          const data = await response.json();
  
          if (selectedView === 'Day') {
              setmembershipCountByCurrentWeek(data);
          } else if (selectedView === 'Month') {
              setMembershipCount(data);
          } else { // selectedView === 'Year'
              setmembershipCountByYearForRecentYears(data);
          }
  
      } catch (error) {
          console.error('Error fetching data:', error);
      }
  };
  

    // Fetch on mount AND when selectedView changes
    useEffect(() => {
        fetchData(); 
    }, [selectedView]);



  // Users
  const [users, setUsers] = useState([]); // State for user data
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const itemsPerPage = 100; // Adjust based on your actual data

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Update the API URL with your endpoint and include pagination parameters if needed
        const response = await fetch(`https://localhost:44395/Users/GetAspUsersPage?currentPage=${currentPage}&itemsPerPage=${itemsPerPage}`);
        const data = await response.json();
        
        setUsers(data.Users); // Adjust this based on the structure of your response
        const totalItems = data.TotalItems; // Adjust this based on your response
        setTotalPages(Math.ceil(totalItems / itemsPerPage));
        console.log(data.Users)
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, [currentPage]); // Re-fetch data when currentPage changes

  const handlePageChange = (newPage) => {
    if (newPage >= 1 && newPage <= totalPages) {
      setCurrentPage(newPage);
    }
  };

  function calculatePagination(currentPage, totalPages) {
    const pageNumbers = [];
    const surroundingPagesLimit = 2; // Số lượng trang xung quanh trang hiện tại để hiển thị
  
    for (let i = 1; i <= totalPages; i++) {
      // Luôn thêm trang đầu tiên và trang cuối cùng
      if (i === 1 || i === totalPages) {
        pageNumbers.push(i);
      }
      // Thêm trang hiện tại và các trang xung quanh
      else if (i >= currentPage - surroundingPagesLimit && i <= currentPage + surroundingPagesLimit) {
        pageNumbers.push(i);
      }
      // Thêm dấu "..." ở giữa các nhóm trang
      else if (i === currentPage - (surroundingPagesLimit + 1) || i === currentPage + (surroundingPagesLimit + 1)) {
        pageNumbers.push('...');
      }
    }
  
    // Loại bỏ các dấu "..." liên tiếp và trùng lặp
    return pageNumbers.filter((value, index, self) => {
      return value !== '...' || (value === '...' && self[index - 1] !== '...');
    });
  }
  





  const random = (min, max) => Math.floor(Math.random() * (max - min + 1) + min)

  const progressExample = [
    { title: 'Visits', value: '29.703 Users', percent: 40, color: 'success' },
    { title: 'Unique', value: '24.093 Users', percent: 20, color: 'info' },
    { title: 'Pageviews', value: '78.706 Views', percent: 60, color: 'warning' },
    { title: 'New Users', value: '22.123 Users', percent: 80, color: 'danger' },
    { title: 'Bounce Rate', value: 'Average Rate', percent: 40.15, color: 'primary' },
  ]

  const progressGroupExample1 = [
    { title: 'Monday', value1: 34, value2: 78 },
    { title: 'Tuesday', value1: 56, value2: 94 },
    { title: 'Wednesday', value1: 12, value2: 67 },
    { title: 'Thursday', value1: 43, value2: 91 },
    { title: 'Friday', value1: 22, value2: 73 },
    { title: 'Saturday', value1: 53, value2: 82 },
    { title: 'Sunday', value1: 9, value2: 99 },
  ]

  const progressGroupExample2 = [
    { title: 'Male', icon: cilUser, value: 53 },
    { title: 'Female', icon: cilUserFemale, value: 43 },
  ]

  const progressGroupExample3 = [
    { title: 'Organic Search', icon: cibGoogle, percent: 56, value: '191,235' },
    { title: 'Facebook', icon: cibFacebook, percent: 15, value: '51,223' },
    { title: 'Twitter', icon: cibTwitter, percent: 11, value: '37,564' },
    { title: 'LinkedIn', icon: cibLinkedin, percent: 8, value: '27,319' },
  ]

  const tableExample = [
    {
      avatar: { src: avatar1, status: 'success' },
      user: {
        name: 'Yiorgos Avraamu',
        new: true,
        registered: 'Jan 1, 2021',
      },
      country: { name: 'USA', flag: cifUs },
      usage: {
        value: 50,
        period: 'Jun 11, 2021 - Jul 10, 2021',
        color: 'success',
      },
      payment: { name: 'Mastercard', icon: cibCcMastercard },
      activity: '10 sec ago',
    },
    {
      avatar: { src: avatar2, status: 'danger' },
      user: {
        name: 'Avram Tarasios',
        new: false,
        registered: 'Jan 1, 2021',
      },
      country: { name: 'Brazil', flag: cifBr },
      usage: {
        value: 22,
        period: 'Jun 11, 2021 - Jul 10, 2021',
        color: 'info',
      },
      payment: { name: 'Visa', icon: cibCcVisa },
      activity: '5 minutes ago',
    },
    {
      avatar: { src: avatar3, status: 'warning' },
      user: { name: 'Quintin Ed', new: true, registered: 'Jan 1, 2021' },
      country: { name: 'India', flag: cifIn },
      usage: {
        value: 74,
        period: 'Jun 11, 2021 - Jul 10, 2021',
        color: 'warning',
      },
      payment: { name: 'Stripe', icon: cibCcStripe },
      activity: '1 hour ago',
    },
    {
      avatar: { src: avatar4, status: 'secondary' },
      user: { name: 'Enéas Kwadwo', new: true, registered: 'Jan 1, 2021' },
      country: { name: 'France', flag: cifFr },
      usage: {
        value: 98,
        period: 'Jun 11, 2021 - Jul 10, 2021',
        color: 'danger',
      },
      payment: { name: 'PayPal', icon: cibCcPaypal },
      activity: 'Last month',
    },
    {
      avatar: { src: avatar5, status: 'success' },
      user: {
        name: 'Agapetus Tadeáš',
        new: true,
        registered: 'Jan 1, 2021',
      },
      country: { name: 'Spain', flag: cifEs },
      usage: {
        value: 22,
        period: 'Jun 11, 2021 - Jul 10, 2021',
        color: 'primary',
      },
      payment: { name: 'Google Wallet', icon: cibCcApplePay },
      activity: 'Last week',
    },
    {
      avatar: { src: avatar6, status: 'danger' },
      user: {
        name: 'Friderik Dávid',
        new: true,
        registered: 'Jan 1, 2021',
      },
      country: { name: 'Poland', flag: cifPl },
      usage: {
        value: 43,
        period: 'Jun 11, 2021 - Jul 10, 2021',
        color: 'success',
      },
      payment: { name: 'Amex', icon: cibCcAmex },
      activity: 'Last week',
    },
  ]

  return (
    <>
      
      <WidgetsDropdown />
      <CCard className="mb-4">
        <CCardBody>
          <CRow>
            <CCol sm={5}>
              <h4 id="traffic" className="card-title mb-0">
                Traffic
              </h4>
              <div className="small text-medium-emphasis">January - July 2021</div>
            </CCol>
            <CCol sm={7} className="d-none d-md-block">
              <CButton color="primary" className="float-end">
                <CIcon icon={cilCloudDownload} />
              </CButton>
              <CButtonGroup className="float-end me-3">
                {['Day', 'Month', 'Year'].map((value) => (
                    <CButton
                        color="outline-secondary"
                        key={value}
                        className="mx-0"
                        active={value === selectedView}
                        onClick={() => setSelectedView(value)} // Update selectedView
                    >
                        {value}
                    </CButton>
                ))}
            </CButtonGroup>
            </CCol>
          </CRow>
          <CChartLine
            style={{ height: '300px', marginTop: '40px' }}
            
            data={{
              labels: selectedView === 'Day'
              ? membershipCountByCurrentWeek.map(item => item.Date) // Labels for days
              : selectedView === 'Month'
              ? ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'] // Labels for months
              : membershipCountByYearForRecentYears.map(item => item.Year), // Labels for years
              datasets: [{
                label: 'My First dataset',
                backgroundColor: hexToRgba(getStyle('--cui-info'), 10),
                borderColor: getStyle('--cui-info'),
                pointHoverBackgroundColor: getStyle('--cui-info'),
                borderWidth: 2,
                data: selectedView === 'Day'
                ? membershipCountByCurrentWeek.map(item => item.TotalCount) 
                : selectedView === 'Month'
                    ? membershipCount.map(item => item.TotalCount) 
                    : membershipCountByYearForRecentYears.map(item => item.TotalCount),  
                  fill: true,
                },
                // {
                //   label: 'My Second dataset',
                //   backgroundColor: 'transparent',
                //   borderColor: getStyle('--cui-success'),
                //   pointHoverBackgroundColor: getStyle('--cui-success'),
                //   borderWidth: 2,
                //   data: [
                //     random(50, 200),
                //     random(50, 200),
                //     random(50, 200),
                //     random(50, 200),
                //     random(50, 200),
                //     random(50, 200),
                //     random(50, 200),
                //   ],
                // },
                // {
                //   label: 'My Third dataset',
                //   backgroundColor: 'transparent',
                //   borderColor: getStyle('--cui-danger'),
                //   pointHoverBackgroundColor: getStyle('--cui-danger'),
                //   borderWidth: 1,
                //   borderDash: [8, 5],
                //   data: [65, 65, 65, 65, 65, 65, 65],
                // },
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
                    drawOnChartArea: false,
                  },
                },
                y: {
                  ticks: {
                    beginAtZero: true,
                    maxTicksLimit: 5,
                    stepSize: Math.ceil(250 / 5),
                    max: 250,
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
        </CCardBody>
        {/* <CCardFooter>
          <CRow xs={{ cols: 1 }} md={{ cols: 5 }} className="text-center">
            {progressExample.map((item, index) => (
              <CCol className="mb-sm-2 mb-0" key={index}>
                <div className="text-medium-emphasis">{item.title}</div>
                <strong>
                  {item.value} ({item.percent}%)
                </strong>
                <CProgress thin className="mt-2" color={item.color} value={item.percent} />
              </CCol>
            ))}
          </CRow>
        </CCardFooter> */}
      </CCard>

      <WidgetsBrand withCharts />

      <CRow>
        <CCol xs>
          {/* <CCard className="mb-4">
            <CCardHeader>Traffic {' & '} Sales</CCardHeader>
            <CCardBody  >
              <CRow>
                <CCol xs={12} md={6} xl={6}>
                  <CRow>
                    <CCol sm={6}>
                      <div className="border-start border-start-4 border-start-info py-1 px-3">
                        <div className="text-medium-emphasis small">New Clients</div>
                        <div className="fs-5 fw-semibold">9,123</div>
                      </div>
                    </CCol>
                    <CCol sm={6}>
                      <div className="border-start border-start-4 border-start-danger py-1 px-3 mb-3">
                        <div className="text-medium-emphasis small">Recurring Clients</div>
                        <div className="fs-5 fw-semibold">22,643</div>
                      </div>
                    </CCol>
                  </CRow>

                  <hr className="mt-0" />
                  {progressGroupExample1.map((item, index) => (
                    <div className="progress-group mb-4" key={index}>
                      <div className="progress-group-prepend">
                        <span className="text-medium-emphasis small">{item.title}</span>
                      </div>
                      <div className="progress-group-bars">
                        <CProgress thin color="info" value={item.value1} />
                        <CProgress thin color="danger" value={item.value2} />
                      </div>
                    </div>
                  ))}
                </CCol>

                <CCol xs={12} md={6} xl={6}>
                  <CRow>
                    <CCol sm={6}>
                      <div className="border-start border-start-4 border-start-warning py-1 px-3 mb-3">
                        <div className="text-medium-emphasis small">Pageviews</div>
                        <div className="fs-5 fw-semibold">78,623</div>
                      </div>
                    </CCol>
                    <CCol sm={6}>
                      <div className="border-start border-start-4 border-start-success py-1 px-3 mb-3">
                        <div className="text-medium-emphasis small">Organic</div>
                        <div className="fs-5 fw-semibold">49,123</div>
                      </div>
                    </CCol>
                  </CRow>

                  <hr className="mt-0" />

                  {progressGroupExample2.map((item, index) => (
                    <div className="progress-group mb-4" key={index}>
                      <div className="progress-group-header">
                        <CIcon className="me-2" icon={item.icon} size="lg" />
                        <span>{item.title}</span>
                        <span className="ms-auto fw-semibold">{item.value}%</span>
                      </div>
                      <div className="progress-group-bars">
                        <CProgress thin color="warning" value={item.value} />
                      </div>
                    </div>
                  ))}

                  <div className="mb-5"></div>

                  {progressGroupExample3.map((item, index) => (
                    <div className="progress-group" key={index}>
                      <div className="progress-group-header">
                        <CIcon className="me-2" icon={item.icon} size="lg" />
                        <span>{item.title}</span>
                        <span className="ms-auto fw-semibold">
                          {item.value}{' '}
                          <span className="text-medium-emphasis small">({item.percent}%)</span>
                        </span>
                      </div>
                      <div className="progress-group-bars">
                        <CProgress thin color="success" value={item.percent} />
                      </div>
                    </div>
                  ))}
                </CCol>
              </CRow>

              <br />
              </CCardBody>
            </CCard> */}

              <CCard>
      <CTable align="middle" className="mb-0 border" hover responsive>
        {/* Fixed Table Header */}
        <CTableHead color="light">
          <CTableRow>
            <CTableHeaderCell className="text-center">
              <CIcon icon={cilPeople} />
            </CTableHeaderCell>
            <CTableHeaderCell>User</CTableHeaderCell>
            {/* <CTableHeaderCell className="text-center">Country</CTableHeaderCell>
            <CTableHeaderCell>Usage</CTableHeaderCell>
            <CTableHeaderCell className="text-center">Payment Method</CTableHeaderCell> */}
            <CTableHeaderCell>Activity</CTableHeaderCell>
          </CTableRow>
        </CTableHead>
      </CTable>
      {/* Scrollable Table Body */}
      <CCardBody    >
        <CTable align="middle" className="mb-0 border" hover responsive>
          <CTableBody>
            {users.map((item, index) => (
              <CTableRow key={index}>
                <CTableDataCell className="text-center">
                  <CAvatar size="md" src={avatar1} status="success" />
                </CTableDataCell>
                <CTableDataCell>
                  <div>{item.UserName}</div>
                  <div className="small text-medium-emphasis">
                    <span>New</span> | Registered: Jan 1, 2021
                  </div>
                </CTableDataCell>
                {/* <CTableDataCell className="text-center">
                  <CIcon size="xl" icon="cifUs" title="USA" />
                </CTableDataCell>
                <CTableDataCell>
                  <div className="clearfix">
                    <div className="float-start">
                      <strong>69%</strong>
                    </div>
                    <div className="float-end">
                      <small className="text-medium-emphasis">
                        Jun 11, 2021 - Jul 10, 2021
                      </small>
                    </div>
                  </div>
                  <CProgress thin color="success" value='50' />
                </CTableDataCell>
                <CTableDataCell className="text-center">
                  <CIcon size="xl" icon="Mastercard" />
                </CTableDataCell> */}
                <CTableDataCell>
                  <div className="small text-medium-emphasis">Last login</div>
                  <strong>{item.LastActivityDate}</strong>
                </CTableDataCell>
              </CTableRow>
            ))}
          </CTableBody>
        </CTable>
      </CCardBody  >
    </CCard>


           
             
             
         
        </CCol>
      </CRow>

      <CPagination aria-label="Page navigation example">
        <CPaginationItem onClick={() => handlePageChange(currentPage - 1)} disabled={currentPage === 1}>
          Previous
        </CPaginationItem>
            {calculatePagination(currentPage, totalPages).map((page, index) => (
        <CPaginationItem
          key={index}
          active={page === currentPage}
          onClick={() => page !== '...' && handlePageChange(page)}
        >
          {page}
        </CPaginationItem>
      ))}
        <CPaginationItem onClick={() => handlePageChange(currentPage + 1)} disabled={currentPage === totalPages}>
          Next
        </CPaginationItem>
      </CPagination>

    </>
  )
}

export default Dashboard
   {/* {tableExample.map((item, index) => (
                    <CTableRow v-for="item in tableItems" key={index}>
                      <CTableDataCell className="text-center">
                        <CAvatar size="md" src={item.avatar.src} status={item.avatar.status} />
                      </CTableDataCell>
                      <CTableDataCell>
                        <div>{item.user.name}</div>
                        <div className="small text-medium-emphasis">
                          <span>{item.user.new ? 'New' : 'Recurring'}</span> | Registered:{' '}
                          {item.user.registered}
                        </div>
                      </CTableDataCell>
                      <CTableDataCell className="text-center">
                        <CIcon size="xl" icon={item.country.flag} title={item.country.name} />
                      </CTableDataCell>
                      <CTableDataCell>
                        <div className="clearfix">
                          <div className="float-start">
                            <strong>{item.usage.value}%</strong>
                          </div>
                          <div className="float-end">
                            <small className="text-medium-emphasis">{item.usage.period}</small>
                          </div>
                        </div>
                        <CProgress thin color={item.usage.color} value={item.usage.value} />
                      </CTableDataCell>
                      <CTableDataCell className="text-center">
                        <CIcon size="xl" icon={item.payment.icon} />
                      </CTableDataCell>
                      <CTableDataCell>
                        <div className="small text-medium-emphasis">Last login</div>
                        <strong>{item.activity}</strong>
                      </CTableDataCell>
                    </CTableRow>
                  ))} */}