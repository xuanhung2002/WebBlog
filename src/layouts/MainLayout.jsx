import React from 'react'
import Header from '../components/Header/Header'
import Footer from '../components/Footer/Footer'
import { Outlet } from 'react-router-dom'
import { Container } from '@mui/material'

export default function MainLayout() {
  return (
    <>
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
      minHeight:"90vh",
      display: "flex",
      flexDirection: "column",
    }}>
    <main style={{ flex: 1 }}>
          <Outlet />
    </main>
    </Container>

    <Footer/>
    </>
  )
}
