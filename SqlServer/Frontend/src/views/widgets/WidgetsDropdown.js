import React from 'react'
import {
  CRow,
  CCol,
  CDropdown,
  CDropdownMenu,
  CDropdownItem,
  CDropdownToggle,
  CWidgetStatsA,
} from '@coreui/react'
import { getStyle } from '@coreui/utils'
import { CChartBar, CChartLine } from '@coreui/react-chartjs'
import CIcon from '@coreui/icons-react'
import { cilArrowBottom, cilArrowTop, cilOptions } from '@coreui/icons'

import  { useState, useEffect } from 'react'

const WidgetsDropdown = () => {

  const [countUsers, setCountUsers] = useState(0); // Initial state: empty array
  const [isLoadingcountUsers, setIsLoadingcountUsers] = useState(true);
  const [errorcountUsers, setErrorcountUsers] = useState(null); 

  //CountUsers countUsers
  const fetchCountUsers = async () => {
    try {
      const url = 'https://localhost:44395/Users/CountUsers';
      const response = await fetch(url);
  
      if (!response.ok) {
        throw new Error(`HTTP Error: ${response.status}`); 
      }
  
      const data = await response.json();
      return data; 
    } catch (error) {
      console.error('Error fetching membership count by year:', error);
      // Handle the error appropriately (e.g., display an error message to the user)
    }
  };

  

  useEffect(() => {
    setIsLoadingcountUsers(true); // Show loading indicator while fetching
    setErrorcountUsers(null); // Clear any previous errors

    fetchCountUsers()
      .then(data => {
        setCountUsers(data.UsersCount);
        setIsLoadingcountUsers(false);
      })
      .catch(error => {
        setErrorcountUsers(error); // Set error state
        setIsLoadingcountUsers(false);
      });
  }, []); // Empty dependency array: Fetch data once on component mount  




  //ApprovedAspUsersCount
  const [approvedAspUsersCount, setapprovedAspUsersCount] = useState(0); // Initial state: empty array
  const [isLoadingApproved, setIsLoadingApproved] = useState(true);
  const [errorApproved, setErrorApproved] = useState(null); 


  const fetchApprovedAspUsersCount = async () => {
    try {
      const url = 'https://localhost:44395/Users/ApprovedAspUsersCount';
      const response = await fetch(url);
  
      if (!response.ok) {
        throw new Error(`HTTP Error: ${response.status}`); 
      }
  
      const data = await response.json();
      return data; 
    } catch (error) {
      console.error('Error fetching membership count by year:', error);
      // Handle the error appropriately (e.g., display an error message to the user)
    }
  };


  useEffect(() => {
    setIsLoadingApproved(true); // Show loading indicator while fetching
    setErrorApproved(null); // Clear any previous errors

    fetchApprovedAspUsersCount()
      .then(data => {
        setapprovedAspUsersCount(data.ApprovedUsersCount);
        setIsLoadingApproved(false);
      })
      .catch(error => {
        setErrorApproved(error); // Set error state
        setapprovedAspUsersCount(false);
      });
  }, []); // Empty dependency array: Fetch data once on component mount  


  //HighFailedPasswordAttemptCount
  const [highFailedPasswordAttemptCount, setHighFailedPasswordAttemptCount] = useState(0); // Initial state: empty array
  const [isLoadingFailedPasswordAttemptCount, setIsLoadingFailedPasswordAttemptCount] = useState(true);
  const [errorFailedPasswordAttemptCount, setFailedPasswordAttemptCount] = useState(null); 


  const fetchHighFailedPasswordAttemptCount = async () => {
    try {
      const url = 'https://localhost:44395/Users/HighFailedPasswordAttemptCount';
      const response = await fetch(url);
  
      if (!response.ok) {
        throw new Error(`HTTP Error: ${response.status}`); 
      }
  
      const data = await response.json();
      return data; 
    } catch (error) {
      console.error('Error fetching membership count by year:', error);
      // Handle the error appropriately (e.g., display an error message to the user)
    }
  };
  

  useEffect(() => {
    setIsLoadingFailedPasswordAttemptCount(true); // Show loading indicator while fetching
    setFailedPasswordAttemptCount(null); // Clear any previous errors

    fetchHighFailedPasswordAttemptCount()
      .then(data => {
      
        setHighFailedPasswordAttemptCount(data.HighFailedPasswordAttemptCount);
  
        setIsLoadingApproved(false);
      })
      .catch(error => {
        setFailedPasswordAttemptCount(error); // Set error state
        setIsLoadingFailedPasswordAttemptCount(false);
      });
  }, []); // Empty dependency array: Fetch data once on component mount  


  
  //LockedOutAspUsersCount
  const [lockedOutAspUsersCount, setlockedOutAspUsersCount] = useState(0); // Initial state: empty array
  const [isLoadingLockedOut, setIsLoadingLockedOut] = useState(true);
  const [errorLockedOut, setErrorLockedOut] = useState(null); 


  const fetchLockedOutAspUsersCount = async () => {
    try {
      const url = 'https://localhost:44395/Users/LockedOutAspUsersCount';
      const response = await fetch(url);
  
      if (!response.ok) {
        throw new Error(`HTTP Error: ${response.status}`); 
      }
  
      const data = await response.json();
      return data; 
    } catch (error) {
      console.error('Error fetching membership count by year:', error);
      // Handle the error appropriately (e.g., display an error message to the user)
    }
  };

  

  useEffect(() => {
    setIsLoadingLockedOut(true); // Show loading indicator while fetching
    setErrorLockedOut(null); // Clear any previous errors

    fetchLockedOutAspUsersCount()
      .then(data => {
       
        setlockedOutAspUsersCount(data.LockedOutAspUsersCount);
      
        setIsLoadingLockedOut(false);
      })
      .catch(error => {
        setErrorLockedOut(error); // Set error state
        setIsLoadingLockedOut(false);
      });
  }, []); // Empty dependency array: Fetch data once on component mount  




  return (
    <CRow>
      <CCol sm={6} lg={3}>
        <CWidgetStatsA
          className="mb-4"
          color="primary"
          value={
            <>
               {countUsers > 0 ? (
                            countUsers // Assuming you want the most recent year
                        ) : (
                            'Loading...' // Or some other placeholder 
                        )} {' '}
              <span className="fs-6 fw-normal">
                {/* (-12.4% <CIcon icon={cilArrowBottom} />) */}
              </span>
            </>
          }
          title="Users"
          action={
            <CDropdown alignment="end">
              <CDropdownToggle color="transparent" caret={false} className="p-0">
                <CIcon icon={cilOptions} className="text-high-emphasis-inverse" />
              </CDropdownToggle>
              <CDropdownMenu>
                <CDropdownItem>Action</CDropdownItem>
                <CDropdownItem>Another action</CDropdownItem>
                <CDropdownItem>Something else here...</CDropdownItem>
                <CDropdownItem disabled>Disabled action</CDropdownItem>
              </CDropdownMenu>
            </CDropdown>
          }
          chart={
            <CChartLine
              className="mt-3 mx-3"
              style={{ height: '70px' }}
              data={{
                labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                datasets: [
                  {
                    label: 'My First dataset',
                    backgroundColor: 'transparent',
                    borderColor: 'rgba(255,255,255,.55)',
                    pointBackgroundColor: getStyle('--cui-primary'),
                    data: [65, 59, 84, 84, 51, 55, 40],
                  },
                ],
              }}
              options={{
                plugins: {
                  legend: {
                    display: false,
                  },
                },
                maintainAspectRatio: false,
                scales: {
                  x: {
                    grid: {
                      display: false,
                      drawBorder: false,
                    },
                    ticks: {
                      display: false,
                    },
                  },
                  y: {
                    min: 30,
                    max: 89,
                    display: false,
                    grid: {
                      display: false,
                    },
                    ticks: {
                      display: false,
                    },
                  },
                },
                elements: {
                  line: {
                    borderWidth: 1,
                    tension: 0.4,
                  },
                  point: {
                    radius: 4,
                    hitRadius: 10,
                    hoverRadius: 4,
                  },
                },
              }}
            />
          }
        />
      </CCol>
      <CCol sm={6} lg={3}>
        <CWidgetStatsA
          className="mb-4"
          color="info"
          value={
            <>
               {approvedAspUsersCount > 0 ? (
                            approvedAspUsersCount// Assuming you want the most recent year
                        ) : (
                            'Loading...' // Or some other placeholder 
                        )} {' '}
              <span className="fs-6 fw-normal">
                {/* (40.9% <CIcon icon={cilArrowTop} />) */}
              </span>
            </>
          }
          title="Approved"
          action={
            <CDropdown alignment="end">
              <CDropdownToggle color="transparent" caret={false} className="p-0">
                <CIcon icon={cilOptions} className="text-high-emphasis-inverse" />
              </CDropdownToggle>
              <CDropdownMenu>
                <CDropdownItem>Action</CDropdownItem>
                <CDropdownItem>Another action</CDropdownItem>
                <CDropdownItem>Something else here...</CDropdownItem>
                <CDropdownItem disabled>Disabled action</CDropdownItem>
              </CDropdownMenu>
            </CDropdown>
          }
          chart={
            <CChartLine
              className="mt-3 mx-3"
              style={{ height: '70px' }}
              data={{
                labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                datasets: [
                  {
                    label: 'My First dataset',
                    backgroundColor: 'transparent',
                    borderColor: 'rgba(255,255,255,.55)',
                    pointBackgroundColor: getStyle('--cui-info'),
                    data: [1, 18, 9, 17, 34, 22, 11],
                  },
                ],
              }}
              options={{
                plugins: {
                  legend: {
                    display: false,
                  },
                },
                maintainAspectRatio: false,
                scales: {
                  x: {
                    grid: {
                      display: false,
                      drawBorder: false,
                    },
                    ticks: {
                      display: false,
                    },
                  },
                  y: {
                    min: -9,
                    max: 39,
                    display: false,
                    grid: {
                      display: false,
                    },
                    ticks: {
                      display: false,
                    },
                  },
                },
                elements: {
                  line: {
                    borderWidth: 1,
                  },
                  point: {
                    radius: 4,
                    hitRadius: 10,
                    hoverRadius: 4,
                  },
                },
              }}
            />
          }
        />
      </CCol>
      <CCol sm={6} lg={3}>
        <CWidgetStatsA
          className="mb-4"
          color="warning"
          value={
            <>
             {highFailedPasswordAttemptCount > 0 ? (
                            highFailedPasswordAttemptCount// Assuming you want the most recent year
                        ) : (
                            'Loading...' // Or some other placeholder 
                        )} {' '}
              <span className="fs-6 fw-normal">
                {/* (84.7% <CIcon icon={cilArrowTop} />) */}
              </span>
            </>
          }
          title="High Failed Password Attempt"
          action={
            <CDropdown alignment="end">
              <CDropdownToggle color="transparent" caret={false} className="p-0">
                <CIcon icon={cilOptions} className="text-high-emphasis-inverse" />
              </CDropdownToggle>
              <CDropdownMenu>
                <CDropdownItem>Action</CDropdownItem>
                <CDropdownItem>Another action</CDropdownItem>
                <CDropdownItem>Something else here...</CDropdownItem>
                <CDropdownItem disabled>Disabled action</CDropdownItem>
              </CDropdownMenu>
            </CDropdown>
          }
          chart={
            <CChartLine
              className="mt-3"
              style={{ height: '70px' }}
              data={{
                labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                datasets: [
                  {
                    label: 'My First dataset',
                    backgroundColor: 'rgba(255,255,255,.2)',
                    borderColor: 'rgba(255,255,255,.55)',
                    data: [78, 81, 80, 45, 34, 12, 40],
                    fill: true,
                  },
                ],
              }}
              options={{
                plugins: {
                  legend: {
                    display: false,
                  },
                },
                maintainAspectRatio: false,
                scales: {
                  x: {
                    display: false,
                  },
                  y: {
                    display: false,
                  },
                },
                elements: {
                  line: {
                    borderWidth: 2,
                    tension: 0.4,
                  },
                  point: {
                    radius: 0,
                    hitRadius: 10,
                    hoverRadius: 4,
                  },
                },
              }}
            />
          }
        />
      </CCol>
      <CCol sm={6} lg={3}>
        <CWidgetStatsA
          className="mb-4"
          color="danger"
          value={
            <>
              {lockedOutAspUsersCount > 0 ? (
                            lockedOutAspUsersCount// Assuming you want the most recent year
                        ) : (
                            'Loading...' // Or some other placeholder 
                        )} {' '}
              <span className="fs-6 fw-normal">
                {/* (-23.6% <CIcon icon={cilArrowBottom} />) */}
              </span>
            </>
          }
          title="locked"
          action={
            <CDropdown alignment="end">
              <CDropdownToggle color="transparent" caret={false} className="p-0">
                <CIcon icon={cilOptions} className="text-high-emphasis-inverse" />
              </CDropdownToggle>
              <CDropdownMenu>
                <CDropdownItem>Action</CDropdownItem>
                <CDropdownItem>Another action</CDropdownItem>
                <CDropdownItem>Something else here...</CDropdownItem>
                <CDropdownItem disabled>Disabled action</CDropdownItem>
              </CDropdownMenu>
            </CDropdown>
          }
          chart={
            <CChartBar
              className="mt-3 mx-3"
              style={{ height: '70px' }}
              data={{
                labels: [
                  'January',
                  'February',
                  'March',
                  'April',
                  'May',
                  'June',
                  'July',
                  'August',
                  'September',
                  'October',
                  'November',
                  'December',
                  'January',
                  'February',
                  'March',
                  'April',
                ],
                datasets: [
                  {
                    label: 'My First dataset',
                    backgroundColor: 'rgba(255,255,255,.2)',
                    borderColor: 'rgba(255,255,255,.55)',
                    data: [78, 81, 80, 45, 34, 12, 40, 85, 65, 23, 12, 98, 34, 84, 67, 82],
                    barPercentage: 0.6,
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
                      display: false,
                      drawTicks: false,
                    },
                    ticks: {
                      display: false,
                    },
                  },
                  y: {
                    grid: {
                      display: false,
                      drawBorder: false,
                      drawTicks: false,
                    },
                    ticks: {
                      display: false,
                    },
                  },
                },
              }}
            />
          }
        />
      </CCol>
    </CRow>
  )
}

export default WidgetsDropdown
