import React from 'react'
import { CFooter } from '@coreui/react'

const AppFooter = () => {
  return (
    <CFooter className="px-4 d-flex justify-content-center">
      <div className="d-flex justify-content-center">
        <span>&copy; 2025 All Rights Reserved by GTÜ BİDB</span>
      </div>

    </CFooter>
  )
}

export default React.memo(AppFooter)
