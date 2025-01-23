import React from 'react'
import CIcon from '@coreui/icons-react'
import {
  cilBell,
  cilCalculator,
  cilChartPie,
  cilCursor,
  cilDescription,
  cilDrop,
  cilExternalLink,
  cilNotes,
  cilPencil,
  cilPuzzle,
  cilSpeedometer,
  cilFire,
  cilDataTransferDown,
  cilLightbulb,
  cilStar,
  cilTrash,
  cilUserPlus,
} from '@coreui/icons'
import { CNavGroup, CNavItem, CNavTitle } from '@coreui/react'

const user = JSON.parse(localStorage.getItem('user'));
const isAdmin = user?.role === 'ADMIN';
console.log(isAdmin + user?.role)

const _nav = [
  {
    component: CNavItem,
    name: 'Electricity',
    to: '/electricity',
    icon: <CIcon icon={cilLightbulb} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Natural Gas ',
    to: '/natural-gas-consumption',
    icon: <CIcon icon={cilFire} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Water ',
    to: '/water-consumption',
    icon: <CIcon icon={cilDrop} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Paper ',
    to: '/paper-consumption',
    icon: <CIcon icon={cilDescription} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Consumption Data Entry',
    to: '/consumption-data-entry',
    icon: <CIcon icon={cilDataTransferDown} customClassName="nav-icon" />,
  },
  // {
  //   component: CNavItem,
  //   name: 'Dashboard',
  //   to: '/dashboard',
  //   icon: <CIcon icon={cilSpeedometer} customClassName="nav-icon" />,
  // },
  {
    component: CNavItem,
    name: 'Carbon Footprint',
    to: '/carbon-footprint',
    icon: <CIcon icon={cilChartPie} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'School Information Form',
    to: '/school-info-form',
    icon: <CIcon icon={cilPencil} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Anomalies',
    to: '/anomalies',
    icon: <CIcon icon={cilBell} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Prediction Results',
    to: '/prediction-results',
    icon: <CIcon icon={cilPuzzle} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Record Management',
    to: '/record-managment',
    icon: <CIcon icon={cilTrash} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'School Info Table',
    to: '/school-info-table',
    icon: <CIcon icon={cilChartPie} customClassName="nav-icon" />,
  },
  ...(isAdmin
    ? [
      {
        component: CNavItem,
        name: 'User Management',
        to: '/user-management',
        icon: <CIcon icon={cilUserPlus} customClassName="nav-icon" />,
      },
    ]
    : []),
]


export default _nav
