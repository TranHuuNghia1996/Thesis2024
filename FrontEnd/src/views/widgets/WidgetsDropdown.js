
import PropTypes from 'prop-types'

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

//reactjs 
import React, { useEffect,useRef ,useState } from 'react';


//

//customer 
import BaseURL from '../../constants/BaseAPI'
//



const WidgetsDropdown = (props) => {
  const widgetChartRef1 = useRef(null)
  const widgetChartRef2 = useRef(null)

  useEffect(() => {
    document.documentElement.addEventListener('ColorSchemeChange', () => {
      if (widgetChartRef1.current) {
        setTimeout(() => {
          widgetChartRef1.current.data.datasets[0].pointBackgroundColor = getStyle('--cui-primary')
          widgetChartRef1.current.update()
        })
      }

      if (widgetChartRef2.current) {
        setTimeout(() => {
          widgetChartRef2.current.data.datasets[0].pointBackgroundColor = getStyle('--cui-info')
          widgetChartRef2.current.update()
        })
      }
    })
  }, [widgetChartRef1, widgetChartRef2])

  // Nghia
  const [userCount, setUserCount] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUserCount = async () => {
      try {
        const response = await fetch(BaseURL+'Memberships/UserCount');
        const data = await response.json();
        setUserCount(data.count);
      } catch (error) {
        console.error('Failed to fetch user count:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserCount();
  }, []);


  const [blockedUserCount, setBlockedUserCount] = useState(null);
  const [loadingBlocked, setLoadingBlocked] = useState(true);

  useEffect(() => {
    const fetchBlockedUserCount = async () => {
      try {
        const response = await fetch(BaseURL+'Memberships/BlockedUserCount');
        const data = await response.json();
        setBlockedUserCount(data.count);
      } catch (error) {
        console.error('Failed to fetch blocked user count:', error);
      } finally {
        setLoadingBlocked(false);
      }
    };

    fetchBlockedUserCount();
  }, []);
  
  //

  return (
    <CRow className={props.className} xs={{ gutter: 4 }}>
      
      <CCol sm={6} xl={4} xxl={3}>
        <CWidgetStatsA
          color="primary"
          value={
            <>
               {loading ? (
        <p>Loading...</p>
      ) : (
        <div>
          <p>{userCount}</p>
         
        </div>
      )} 
            </>
          }
          title="Members"       
        />
      </CCol>

      <CCol sm={6} xl={4} xxl={3}>
        <CWidgetStatsA
          color="danger"
          value={
            <>
               {loadingBlocked ? (
        <p>Loading...</p>
      ) : (
        <div>
          <p>{blockedUserCount}</p>
         
        </div>
      )} 
            </>
          }
          title="Blocked"       
        />
      </CCol>

   
    </CRow>
  )
}

WidgetsDropdown.propTypes = {
  className: PropTypes.string,
  withCharts: PropTypes.bool,
}

export default WidgetsDropdown
