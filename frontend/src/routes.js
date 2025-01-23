import React from 'react'

const Electiricty = React.lazy(() => import('./views/electricity/Electricity'))
const ElectricityConsumptionAI = React.lazy(() => import('./views/electricity/ElectricityConsumptionAI'))
const NaturalGasConsumption = React.lazy(() => import('./views/natural-gas/NaturalGasConsumption'))
const WaterConsumption = React.lazy(() => import('./views/water/WaterConsumption'))
const AnomaliesDetectionPage = React.lazy(() => import('./views/anormalies/AnomaliesDetectionPage'))
const EmissionDataPage = React.lazy(() => import('./views/carbon-footprint/EmissionDataPage'))
const SchoolInfoForm = React.lazy(() => import('./views/school/SchoolInfoForm'))
const ConsumptionInputPage = React.lazy(() => import('./views/input/ConsumptionInputPage'))
const PredictionResult = React.lazy(() => import('./views/prediction/PredictionResult'))
const DeleteRecordPage = React.lazy(() => import('./views/delete-record-pages/DeleteRecordPage'))
const Dashboard = React.lazy(() => import('./views/dashboard/Dashboard'))
const PaperConsumption = React.lazy(() => import('./views/paper/PaperConsumption'))
const UserCreatePage = React.lazy(() => import('./views/user/UserCreatePage'))
const SchoolInfoTable = React.lazy(() => import('./views/school/SchoolInfoTable'))

const Colors = React.lazy(() => import('./views/theme/colors/Colors'))
const Typography = React.lazy(() => import('./views/theme/typography/Typography'))

// Base
const Accordion = React.lazy(() => import('./views/base/accordion/Accordion'))
const Breadcrumbs = React.lazy(() => import('./views/base/breadcrumbs/Breadcrumbs'))
const Cards = React.lazy(() => import('./views/base/cards/Cards'))
const Carousels = React.lazy(() => import('./views/base/carousels/Carousels'))
const Collapses = React.lazy(() => import('./views/base/collapses/Collapses'))
const ListGroups = React.lazy(() => import('./views/base/list-groups/ListGroups'))
const Navs = React.lazy(() => import('./views/base/navs/Navs'))
const Paginations = React.lazy(() => import('./views/base/paginations/Paginations'))
const Placeholders = React.lazy(() => import('./views/base/placeholders/Placeholders'))
const Popovers = React.lazy(() => import('./views/base/popovers/Popovers'))
const Progress = React.lazy(() => import('./views/base/progress/Progress'))
const Spinners = React.lazy(() => import('./views/base/spinners/Spinners'))
const Tabs = React.lazy(() => import('./views/base/tabs/Tabs'))
const Tables = React.lazy(() => import('./views/base/tables/Tables'))
const Tooltips = React.lazy(() => import('./views/base/tooltips/Tooltips'))

// Buttons
const Buttons = React.lazy(() => import('./views/buttons/buttons/Buttons'))
const ButtonGroups = React.lazy(() => import('./views/buttons/button-groups/ButtonGroups'))
const Dropdowns = React.lazy(() => import('./views/buttons/dropdowns/Dropdowns'))

//Forms
const ChecksRadios = React.lazy(() => import('./views/forms/checks-radios/ChecksRadios'))
const FloatingLabels = React.lazy(() => import('./views/forms/floating-labels/FloatingLabels'))
const FormControl = React.lazy(() => import('./views/forms/form-control/FormControl'))
const InputGroup = React.lazy(() => import('./views/forms/input-group/InputGroup'))
const Layout = React.lazy(() => import('./views/forms/layout/Layout'))
const Range = React.lazy(() => import('./views/forms/range/Range'))
const Select = React.lazy(() => import('./views/forms/select/Select'))
const Validation = React.lazy(() => import('./views/forms/validation/Validation'))

const Charts = React.lazy(() => import('./views/charts/Charts'))

// Icons
const CoreUIIcons = React.lazy(() => import('./views/icons/coreui-icons/CoreUIIcons'))
const Flags = React.lazy(() => import('./views/icons/flags/Flags'))
const Brands = React.lazy(() => import('./views/icons/brands/Brands'))

// Notifications
const Alerts = React.lazy(() => import('./views/notifications/alerts/Alerts'))
const Badges = React.lazy(() => import('./views/notifications/badges/Badges'))
const Modals = React.lazy(() => import('./views/notifications/modals/Modals'))
const Toasts = React.lazy(() => import('./views/notifications/toasts/Toasts'))

const Widgets = React.lazy(() => import('./views/widgets/Widgets'))


const user = JSON.parse(localStorage.getItem('user'));
const isAdmin = user?.role === 'ADMIN';

const routes = [
  { path: '/electricity', exact: true, name: 'Home' },
  { path: '/consumption-data-entry', name: 'Consumption Data Entry', element: ConsumptionInputPage },
  { path: '/electricity', name: 'Electricity', element: Electiricty },
  { path: '/natural-gas-consumption', name: 'Natural Gas Consumption', element: NaturalGasConsumption },
  { path: '/water-consumption', name: 'Water Consumption', element: WaterConsumption },
  { path: '/paper-consumption', name: 'Paper Consumption', element: PaperConsumption },
  { path: '/carbon-footprint', name: 'Carbon Footprint', element: EmissionDataPage },
  { path: '/school-info-form', name: 'School Information Form', element: SchoolInfoForm },
  { path: '/anomalies', name: 'Anomalies', element: AnomaliesDetectionPage },
  { path: '/prediction-results', name: 'Prediction Results', element: PredictionResult },
  { path: '/record-managment', name: 'Delete Records', element: DeleteRecordPage },
  // { path: '/dashboard', name: 'Dashboard', element: Dashboard },
  {path: '/school-info-table', name: 'School Info Table', element: SchoolInfoTable},
  ...(isAdmin
    ? [{ path: '/user-management', name: 'User Management', element: UserCreatePage }]
    : []),
]


export default routes
