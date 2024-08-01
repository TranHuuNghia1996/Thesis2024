import React, { useState, useEffect } from 'react';
import { CTable, CTableHead, CTableRow, CTableHeaderCell, CTableBody, CTableDataCell, CPagination, CPaginationItem, CDropdown, CDropdownToggle, CDropdownMenu, CDropdownItem, CFormInput, CRow, CCol } from '@coreui/react';
import BaseURL from '../../constants/BaseAPI';

const UserTable = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [filter, setFilter] = useState('recent');
  const [filterTitle, setFilterTitle] = useState('Recent Logins');
  const [searchQuery, setSearchQuery] = useState('');
  const pageSize = 100;

  useEffect(() => {
    fetchUsers(currentPage, filter, searchQuery);
  }, [currentPage, filter, searchQuery]);

  const fetchUsers = async (page, filter, searchQuery) => {
    setLoading(true);
    try {
      const response = await fetch(`${BaseURL}/Memberships/PagedUsers?pageNumber=${page}&pageSize=${pageSize}&filter=${filter}&searchQuery=${searchQuery}`);
      const result = await response.json();
      if (result && result.users) {
        setUsers(result.users);
        setTotalPages(Math.ceil(result.totalCount / pageSize));
      } else {
        setUsers([]);
        setTotalPages(1);
      }
    } catch (error) {
      console.error('Failed to fetch users:', error);
      setUsers([]);
      setTotalPages(1);
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const handleFilterChange = (filter, title) => {
    setFilter(filter);
    setFilterTitle(title);
    setCurrentPage(1);
  };

  const handleSearchChange = (e) => {
    setSearchQuery(e.target.value);
    setCurrentPage(1); // Reset to first page on new search
  };

  const renderPaginationItems = () => {
    let items = [];

    if (currentPage > 2) {
      items.push(
        <CPaginationItem key={1} onClick={() => handlePageChange(1)}>
          1
        </CPaginationItem>
      );
      if (currentPage > 3) {
        items.push(<CPaginationItem key="ellipsis1" disabled>...</CPaginationItem>);
      }
    }

    if (currentPage > 1) {
      items.push(
        <CPaginationItem key={currentPage - 1} onClick={() => handlePageChange(currentPage - 1)}>
          {currentPage - 1}
        </CPaginationItem>
      );
    }

    items.push(
      <CPaginationItem key={currentPage} active>
        {currentPage}
      </CPaginationItem>
    );

    if (currentPage < totalPages) {
      items.push(
        <CPaginationItem key={currentPage + 1} onClick={() => handlePageChange(currentPage + 1)}>
          {currentPage + 1}
        </CPaginationItem>
      );
    }

    if (currentPage < totalPages - 1) {
      if (currentPage < totalPages - 2) {
        items.push(<CPaginationItem key="ellipsis2" disabled>...</CPaginationItem>);
      }
      items.push(
        <CPaginationItem key={totalPages} onClick={() => handlePageChange(totalPages)}>
          {totalPages}
        </CPaginationItem>
      );
    }

    return items;
  };

  return (
    <div>
      <CDropdown className="mb-3">
        <CDropdownToggle color="secondary">{filterTitle}</CDropdownToggle>
        <CDropdownMenu>
          <CDropdownItem onClick={() => handleFilterChange('recent', 'Recent Logins')}>Recent Logins</CDropdownItem>
          <CDropdownItem onClick={() => handleFilterChange('blocked', 'Blocked Users')}>Blocked Users</CDropdownItem>
          <CDropdownItem onClick={() => handleFilterChange('notloggedinmonth', 'Not Logged In Last Month')}>Not Logged In Last Month</CDropdownItem>
        </CDropdownMenu>
      </CDropdown>
      <CRow className="mb-3">
        <CCol md="9">
          <CFormInput 
            type="text" 
            placeholder="Search by name or email" 
            value={searchQuery} 
            onChange={handleSearchChange} 
          />
        </CCol>
      </CRow>
      <div className="table-wrapper">
        <div className="table-content">
          <CTable align="middle" className="mb-0 border" hover responsive>
            <CTableHead className="text-nowrap">
              <CTableRow>
                <CTableHeaderCell>No</CTableHeaderCell>
                <CTableHeaderCell>User Name</CTableHeaderCell>
                <CTableHeaderCell>First Name</CTableHeaderCell>
                <CTableHeaderCell>Last Name</CTableHeaderCell>
                <CTableHeaderCell>Email</CTableHeaderCell>
                <CTableHeaderCell>Last Login</CTableHeaderCell>
              </CTableRow>
            </CTableHead>
            <CTableBody>
              {loading ? (
                <CTableRow>
                  <CTableDataCell colSpan="6" className="text-center">Loading...</CTableDataCell>
                </CTableRow>
              ) : (
                users.map((user, index) => (
                  <CTableRow key={index}>
                    <CTableDataCell>{(currentPage - 1) * pageSize + index + 1}</CTableDataCell>
                    <CTableDataCell>{user.UserName}</CTableDataCell>
                    <CTableDataCell>{user.FirstName}</CTableDataCell>
                    <CTableDataCell>{user.LastName}</CTableDataCell>
                    <CTableDataCell>{user.Email}</CTableDataCell>
                    <CTableDataCell>{new Date(user.LastLoginDate).toLocaleString()}</CTableDataCell>
                  </CTableRow>
                ))
              )}
            </CTableBody>
          </CTable>
        </div>
      </div>
      <CPagination align="end" className="mt-3">
        <CPaginationItem disabled={currentPage === 1} onClick={() => handlePageChange(1)}>First</CPaginationItem>
        <CPaginationItem disabled={currentPage === 1} onClick={() => handlePageChange(currentPage - 1)}>Previous</CPaginationItem>
        {renderPaginationItems()}
        <CPaginationItem disabled={currentPage >= totalPages} onClick={() => handlePageChange(currentPage + 1)}>Next</CPaginationItem>
        <CPaginationItem disabled={currentPage >= totalPages} onClick={() => handlePageChange(totalPages)}>Last</CPaginationItem>
      </CPagination>
    </div>
  );
};

export default UserTable;
