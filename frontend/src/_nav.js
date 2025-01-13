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
    name: 'Elektrik',
    to: '/elektrik',
    icon: <CIcon icon={cilLightbulb} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Doğalgaz',
    to: '/dogalgaz-tuketim',
    icon: <CIcon icon={cilFire} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Su Tüketim',
    to: '/su-tuketim',
    icon: <CIcon icon={cilDrop} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Kagıt Tüketim',
    to: '/kagit-tuketim',
    icon: <CIcon icon={cilDescription} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Tüketim Veri Girişi',
    to: '/tüketim-veri-girisi',
    icon: <CIcon icon={cilDataTransferDown} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Dashboard',
    to: '/dashboard',
    icon: <CIcon icon={cilSpeedometer} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Karbon Ayak İzi',
    to: '/karbon-ayakizi',
    icon: <CIcon icon={cilChartPie} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Okul Bilgi Formu',
    to: '/okul-bilgi-formu',
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
    name: 'Tahmin Sonuc',
    to: '/tahmin-sonuc',
    icon: <CIcon icon={cilPuzzle} customClassName="nav-icon" />,
  },
  {
    component: CNavItem,
    name: 'Kayıt Silme',
    to: '/kayıt-silme',
    icon: <CIcon icon={cilTrash} customClassName="nav-icon" />,
  },
  ...(isAdmin
    ? [
      {
        component: CNavItem,
        name: 'Kulanıcı Oluştur',
        to: '/kulanicilar-olustur',
        icon: <CIcon icon={cilUserPlus} customClassName="nav-icon" />,
      },
    ]
    : []),

]

export default _nav
