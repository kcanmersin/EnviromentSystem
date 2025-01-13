// src/components/LogoutButton.js
import React from 'react'
import { useNavigate } from 'react-router-dom'
import { CButton } from '@coreui/react'
import CIcon from '@coreui/icons-react'
import { cilAccountLogout } from '@coreui/icons'

const LogoutButton = () => {
  const navigate = useNavigate()

  const handleLogout = () => {
    localStorage.removeItem('token')
    localStorage.removeItem('user')
    navigate('/login')
  }

  return <CButton onClick={handleLogout}>
    <CIcon icon={cilAccountLogout} />
  </CButton>
}

export default LogoutButton
