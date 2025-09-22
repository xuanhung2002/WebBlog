import { Box, Container, Typography } from '@mui/material';
import React from 'react'

export default function Footer() {
  return (
    <Box
      sx={{
        position: 'absolute',   
        bottom: 0,
        width: '100%',       
        backgroundColor: '#333',
        color: 'white',
        padding: '10px 0',
      }}
    >
      <Container maxWidth="lg" sx={{ textAlign: 'center' }}>
        <Typography variant="body2">
          © 2025 Nxhung02 Website. All Rights Reserved.
        </Typography>
      </Container>
    </Box>
  );
}
