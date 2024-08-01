import React from 'react'
import PropTypes from 'prop-types'
import { CWidgetStatsD, CRow, CCol } from '@coreui/react'
import CIcon from '@coreui/icons-react'
import { cibFacebook, cibLinkedin, cibTwitter, cilCalendar,cibTiktok,cibInstagram } from '@coreui/icons'
import { CChart } from '@coreui/react-chartjs'

import  { useState, useEffect } from 'react'

const WidgetsBrand = ({ withCharts }) => {
  const chartOptions = {
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
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: false,
      },
    },
    scales: {
      x: {
        display: false,
      },
      y: {
        display: false,
      },
    },
    
  }


   //UsersWithTwitterCommentsCount

   const [usersWithTwitterCommentsCount, setUsersWithTwitterCommentsCount] = useState(0); // Initial state: empty array
   const [isLoadingUsersWithTwitterCommentsCount, setIsLoadingUsersWithTwitterCommentsCount] = useState(true);
   const [errorUsersWithTwitterCommentsCount, setErrorUsersWithTwitterCommentsCount] = useState(null); 

   const fetchUsersWithTwitterCommentsCount = async () => {
    try {
      const url = 'https://localhost:44395/Users/UsersWithTwitterCommentsCount';
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
    setIsLoadingUsersWithTwitterCommentsCount(true); // Show loading indicator while fetching
    setErrorUsersWithTwitterCommentsCount(null); // Clear any previous errors

    fetchUsersWithTwitterCommentsCount()
      .then(data => {
        setUsersWithTwitterCommentsCount(data.UsersWithTwitterCommentsCount);
        setIsLoadingUsersWithTwitterCommentsCount(false);
      })
      .catch(error => {
        setErrorUsersWithTwitterCommentsCount(error); // Set error state
        setIsLoadingUsersWithTwitterCommentsCount(false);
      });
  }, []); // Empty dependency array: Fetch data once on component mount  

    //UsersWithFaceBookCommentsAndMobileAliasCount

    const [usersWithFaceBookCommentsAndMobileAliasCount, setUsersWithFaceBookCommentsAndMobileAliasCount] = useState(0); // Initial state: empty array
    const [isLoadingUsersWithFaceBookCommentsAndMobileAliasCount, setIsLoadingUsersWithFaceBookCommentsAndMobileAliasCount] = useState(true);
    const [errorUsersWithFaceBookCommentsAndMobileAliasCount, setErrorUsersWithFaceBookCommentsAndMobileAliasCount] = useState(null); 
 
    const fetchUsersWithFaceBookCommentsAndMobileAliasCount = async () => {
     try {
       const url = 'https://localhost:44395/Users/UsersWithFaceBookCommentsAndMobileAliasCount';
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
    setIsLoadingUsersWithFaceBookCommentsAndMobileAliasCount(true); // Show loading indicator while fetching
     setErrorUsersWithFaceBookCommentsAndMobileAliasCount(null); // Clear any previous errors
 
     fetchUsersWithFaceBookCommentsAndMobileAliasCount()
       .then(data => {
        setUsersWithFaceBookCommentsAndMobileAliasCount(data.UsersWithFaceBookCommentsAndMobileAliasCount);
         setIsLoadingUsersWithFaceBookCommentsAndMobileAliasCount(false);
       })
       .catch(error => {
        setErrorUsersWithFaceBookCommentsAndMobileAliasCount(error); // Set error state
         setIsLoadingUsersWithFaceBookCommentsAndMobileAliasCount(false);
       });
   }, []); // Empty dependency array: Fetch data once on component mount  



  //UsersWithFaceBookCommentsCount


  const [usersWithFaceBookCommentsCount, setUsersWithFaceBookCommentsCount] = useState(0); // Initial state: empty array
   const [isLoadingUsersWithFaceBookCommentsCount, setIsLoadingUsersWithFaceBookCommentsCount] = useState(true);
   const [errorUsersWithFaceBookCommentsCount, setErrorUsersWithFaceBookCommentsCount] = useState(null); 

   const fetchUsersWithFaceBookCommentsCount = async () => {
    try {
      const url = 'https://localhost:44395/Users/UsersWithFaceBookCommentsCount';
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
    setIsLoadingUsersWithFaceBookCommentsCount(true); // Show loading indicator while fetching
    setErrorUsersWithFaceBookCommentsCount(null); // Clear any previous errors

    fetchUsersWithFaceBookCommentsCount()
      .then(data => {
        setUsersWithFaceBookCommentsCount(data.UsersWithFaceBookCommentsCount);
        setIsLoadingUsersWithFaceBookCommentsCount(false);
      })
      .catch(error => {
        setErrorUsersWithFaceBookCommentsCount(error); // Set error state
        setIsLoadingUsersWithFaceBookCommentsCount(false);
      });
  }, []); // Empty dependency array: Fetch data once on component mount  


    //UsersWithTwitterCommentsAndMobileAliasCount


    const [usersWithTwitterCommentsAndMobileAliasCount, setUsersWithTwitterCommentsAndMobileAliasCount] = useState(0); // Initial state: empty array
    const [isLoadingUsersWithTwitterCommentsAndMobileAliasCount, setIsLoadingUsersWithTwitterCommentsAndMobileAliasCount] = useState(true);
    const [errorUsersWithTwitterCommentsAndMobileAliasCount, setErrorUsersWithTwitterCommentsAndMobileAliasCount] = useState(null); 
 
    const fetchUsersWithTwitterCommentsAndMobileAliasCount = async () => {
     try {
       const url = 'https://localhost:44395/Users/UsersWithTwitterCommentsAndMobileAliasCount';
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
    setIsLoadingUsersWithTwitterCommentsAndMobileAliasCount(true); // Show loading indicator while fetching
    setErrorUsersWithTwitterCommentsAndMobileAliasCount(null); // Clear any previous errors
 
     fetchUsersWithTwitterCommentsAndMobileAliasCount()
       .then(data => {
        setUsersWithTwitterCommentsAndMobileAliasCount(data.UsersWithTwitterCommentsAndMobileAliasCount);
        setIsLoadingUsersWithTwitterCommentsAndMobileAliasCount(false);
       })
       .catch(error => {
        setErrorUsersWithTwitterCommentsAndMobileAliasCount(error); // Set error state
         setIsLoadingUsersWithTwitterCommentsAndMobileAliasCount(false);
       });
   }, []); // Empty dependency array: Fetch data once on component mount  



    //UsersWithInstagramCommentsCount


    const [usersWithInstagramCommentsCount, setUsersWithInstagramCommentsCount] = useState(0); // Initial state: empty array
    const [isLoadingUsersWithInstagramCommentsCount, setIsLoadingUsersWithInstagramCommentsCount] = useState(true);
    const [errorUsersWithInstagramCommentsCount, setErrorUsersWithInstagramCommentsCount] = useState(null); 
 
    const fetchUsersWithInstagramCommentsCount = async () => {
     try {
       const url = 'https://localhost:44395/Users/UsersWithInstagramCommentsCount';
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

   //UsersWithTikTokCommentsCount
   const [usersWithTikTokCommentsCount, setusersWithTikTokCommentsCount] = useState(0); // Initial state: empty array
   const [isLoadingusersWithTikTokCommentsCount, setIsLoadingusersWithTikTokCommentsCount] = useState(true);
   const [errorusersWithTikTokCommentsCount, setErrorusersWithTikTokCommentsCount] = useState(null); 

   const fetchusersWithTikTokCommentsCount = async () => {
    try {
      const url = 'https://localhost:44395/Users/UsersWithTikTokCommentsCount';
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
    setIsLoadingusersWithTikTokCommentsCount(true); // Show loading indicator while fetching
    setErrorusersWithTikTokCommentsCount(null); // Clear any previous errors
 
    fetchusersWithTikTokCommentsCount()
       .then(data => {
        setusersWithTikTokCommentsCount(data.UsersWithTikTokCommentsCount);
        setIsLoadingusersWithTikTokCommentsCount(false);
       })
       .catch(error => {
        setErrorusersWithTikTokCommentsCount(error); // Set error state
        setIsLoadingusersWithTikTokCommentsCount(false);
       });
   }, []); // Empty dependency array: Fetch data once on component mount  
 

   //UsersWithInstagramCommentsAndMobileAliasCount


   const [usersWithInstagramCommentsAndMobileAliasCount, setUsersWithInstagramCommentsAndMobileAliasCount] = useState(0); // Initial state: empty array
   const [isLoadingUsersWithInstagramCommentsAndMobileAliasCount, setIsLoadingUsersWithInstagramCommentsAndMobileAliasCount] = useState(true);
   const [errorUsersWithInstagramCommentsAndMobileAliasCount, setErrorUsersWithInstagramCommentsAndMobileAliasCount] = useState(null); 

   const fetchUsersWithInstagramCommentsAndMobileAliasCount = async () => {
    try {
      const url = 'https://localhost:44395/Users/UsersWithInstagramCommentsAndMobileAliasCount';
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
    setIsLoadingUsersWithInstagramCommentsAndMobileAliasCount(true); // Show loading indicator while fetching
    setErrorUsersWithInstagramCommentsAndMobileAliasCount(null); // Clear any previous errors

    fetchUsersWithInstagramCommentsAndMobileAliasCount()
      .then(data => {
        setUsersWithInstagramCommentsAndMobileAliasCount(data.UsersWithInstagramCommentsAndMobileAliasCount);
       setUsersWithInstagramCommentsCount(false);
      })
      .catch(error => {
        setErrorUsersWithInstagramCommentsAndMobileAliasCount(error); // Set error state
       setIsLoadingUsersWithInstagramCommentsAndMobileAliasCount(false);
      });
  }, []); // Empty dependency array: Fetch data once on component mount  


  //UsersWithTikTokCommentsAndMobileAliasCount


  const [usersWithTikTokCommentsAndMobileAliasCount, setUsersWithTikTokCommentsAndMobileAliasCount] = useState(0); // Initial state: empty array
  const [isLoadingUsersWithTikTokCommentsAndMobileAliasCount, setIsLoadingUsersWithTikTokCommentsAndMobileAliasCount] = useState(true);
  const [errorUsersWithTikTokCommentsAndMobileAliasCount, setErrorUsersWithTikTokCommentsAndMobileAliasCount] = useState(null); 

  const fetchUsersWithTikTokCommentsAndMobileAliasCount = async () => {
   try {
     const url = 'https://localhost:44395/Users/UsersWithTikTokCommentsAndMobileAliasCount';
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
  setIsLoadingUsersWithTikTokCommentsAndMobileAliasCount(true); // Show loading indicator while fetching
  setErrorUsersWithTikTokCommentsAndMobileAliasCount(null); // Clear any previous errors

   fetchUsersWithTikTokCommentsAndMobileAliasCount()
     .then(data => {
      setUsersWithTikTokCommentsAndMobileAliasCount(data.UsersWithTikTokCommentsAndMobileAliasCount);
      setIsLoadingUsersWithTikTokCommentsAndMobileAliasCount(false);
     })
     .catch(error => {
      setErrorUsersWithTikTokCommentsAndMobileAliasCount(error); // Set error state
       setIsLoadingUsersWithTikTokCommentsAndMobileAliasCount(false);
     });
 }, []); // Empty dependency array: Fetch data once on component mount  


  return (
    <CRow>
      <CCol sm={6} lg={3}>
        <CWidgetStatsD
          className="mb-4"
          {...(withCharts && {
            chart: (
              <CChart
                className="position-absolute w-100 h-100"
                type="line"
                data={{
                  labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                  datasets: [
                    {
                      backgroundColor: 'rgba(255,255,255,.1)',
                      borderColor: 'rgba(255,255,255,.55)',
                      pointHoverBackgroundColor: '#fff',
                      borderWidth: 2,
                     
                      fill: true,
                    },
                  ],
                }}
                options={chartOptions}
              />
            ),
          })}
          icon={<CIcon icon={cibFacebook} height={52} className="my-4 text-white" />}
          values={[
            { title: 'Comment', value: usersWithFaceBookCommentsCount > 0 ? (
              usersWithFaceBookCommentsCount// Assuming you want the most recent year
          ) : (
              'Loading...' // Or some other placeholder 
          )},
            { title: 'MobileAlias', value: usersWithFaceBookCommentsAndMobileAliasCount > 0 ? (
              usersWithFaceBookCommentsAndMobileAliasCount// Assuming you want the most recent year
          ) : (
              'Loading...' // Or some other placeholder 
          )},
          ]}
          style={{
            '--cui-card-cap-bg': '#3b5998',
          }}
        />
      </CCol>

      <CCol sm={6} lg={3}>
        <CWidgetStatsD
          className="mb-4"
          {...(withCharts && {
            chart: (
              <CChart
                className="position-absolute w-100 h-100"
                type="line"
                data={{
                  labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                  datasets: [
                    {
                      backgroundColor: 'rgba(255,255,255,.1)',
                      borderColor: 'rgba(255,255,255,.55)',
                      pointHoverBackgroundColor: '#fff',
                      borderWidth: 2,
                    
                      fill: true,
                    },
                  ],
                }}
                options={chartOptions}
              />
            ),
          })}
          icon={<CIcon icon={cibTwitter} height={52} className="my-4 text-white" />}
          values={[
            { title: 'Comment', value: usersWithTwitterCommentsCount > 0 ? (
              usersWithTwitterCommentsCount// Assuming you want the most recent year
          ) : (
              'Loading...' // Or some other placeholder 
          ) },
            { title: 'MobileAlias', value: usersWithTwitterCommentsAndMobileAliasCount > 0 ? (
              usersWithTwitterCommentsAndMobileAliasCount// Assuming you want the most recent year
          ) : (
              'Loading...' // Or some other placeholder 
          ) },
          ]}
          style={{
            '--cui-card-cap-bg': '#00aced',
          }}
        />
      </CCol>

      <CCol sm={6} lg={3}>
        <CWidgetStatsD
          className="mb-4"
          color="Info"
          icon={<CIcon icon={cibInstagram} height={52} className="my-4 text-white" />}
          values={[
            { title: 'Comment', value: usersWithTwitterCommentsCount > 0 ? (
              usersWithTwitterCommentsCount// Assuming you want the most recent year
          ) : (
              'Loading...' // Or some other placeholder 
          ) },
            { title: 'MobileAlias', value: usersWithInstagramCommentsAndMobileAliasCount > 0 ? (
              usersWithInstagramCommentsAndMobileAliasCount// Assuming you want the most recent year
          ) : (
              'Loading...' // Or some other placeholder 
          )  },
          ]}
          style={{
            '--cui-card-cap-bg': '#bc2a8d',
          }}
        />
      </CCol>

      <CCol sm={6} lg={3}>
        <CWidgetStatsD
          className="mb-4"
          color="Dark"
          {...(withCharts && {
            chart: (
              <CChart
                className="position-absolute w-100 h-100"
                type="line"
                data={{
                  labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
                  datasets: [
                    {
                      backgroundColor: 'rgba(255,255,255,.1)',
                      borderColor: 'rgba(255,255,255,.55)',
                      pointHoverBackgroundColor: '#fff',
                      borderWidth: 2,
                     
                      fill: true,
                    },
                  ],
                }}
                options={chartOptions}
              />
            ),
          })}
          icon={<CIcon icon={cibTiktok} height={52} className="my-4 text-white" />}
          values={[
            { title: 'Comment', value: usersWithTikTokCommentsCount > 0 ? (
              usersWithTikTokCommentsCount// Assuming you want the most recent year
          ) : (
              'Loading...' // Or some other placeholder 
          )  },
            { title: 'MobileAlias', value: usersWithTikTokCommentsAndMobileAliasCount > 0 ? (
              usersWithTikTokCommentsAndMobileAliasCount// Assuming you want the most recent year
          ) : (
              'Loading...' // Or some other placeholder 
          ) },
          ]}
          style={{
            '--cui-card-cap-bg': '#e95950',
          }}
        />
      </CCol>
    </CRow>
  )
}

WidgetsBrand.propTypes = {
  withCharts: PropTypes.bool,
}

export default WidgetsBrand
