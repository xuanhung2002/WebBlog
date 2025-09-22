import React from 'react'
import Header from '../components/Header/Header'
import { Outlet } from 'react-router-dom'
import { Container, minHeight } from '@mui/system'

export default function AuthLayout() {
  return (
    <div>
    <Header/>
    <Container
     sx={{
      maxWidth: {
        xs: '100%',
        sm: '100%',  
        md: '80%',   
        lg: '75%',  
        xl: '70%',   
      },
      minHeight:"100%"
    }}>
    <Outlet/>
    </Container>
    </div>
  )
}
