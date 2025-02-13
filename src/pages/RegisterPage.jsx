import { Facebook } from '@mui/icons-material';
import { Avatar, Box, Button, Checkbox, Container, Divider, FormControlLabel, TextField, Typography } from '@mui/material';
import React, { useState } from 'react'
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { Link } from 'react-router-dom';
import GoogleIcon from "../assets/icons/googleicon.png";

const RegisterPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  
  const handleSubmit = (event) => {
    event.preventDefault();
    // Xử lý logic đăng nhập ở đây
    console.log('Email:', email);
    console.log('Password:', password);
  };

  return (
    <Container component="main" maxWidth="xs">
      <Box
        sx={{
          marginTop: 8,
          display: "flex",
          flexDirection: "column",
        }}
      >
        <Avatar sx={{ m: 1, bgcolor: "secondary.main", alignSelf: "center" }}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5" sx={{ alignSelf: "center" }}>
          Sign Up
        </Typography>
        <Box
          component="form"
          onSubmit={handleSubmit}
          noValidate
          sx={{
            mt: 1,
            display: "flex",
            flexDirection: "column",
          }}
        >
            <TextField
            margin="normal"
            required
            fullWidth
            id="fullname"
            label="Full name"
            name="fullname"
            autoComplete="none"
            autoFocus
            value={email}
            // onChange={(e) => setEmail(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            id="email"
            label="Email Address"
            name="email"
            autoComplete="email"           
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="password"
            label="Password"
            type="password"
            id="password"
            autoComplete="current-password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />         
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Sign Up
          </Button>        

          <Box sx={{ display: "flex", alignItems: "center", mt: 2 }}>
            <Divider sx={{ flexGrow: 1 }} />
            <Box sx={{ mx: 2 }}>or</Box>
            <Divider sx={{ flexGrow: 1 }} />
          </Box>

          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
            }}
          >
            <Button
              fullWidth
              variant="outlined"
              sx={{ mt: 2, mb: 1, textTransform: "none" }}
            >
              <img
                src={GoogleIcon} // Thay đường dẫn icon Google của bạn
                alt="Google"
                style={{ width: 20, height: 20, marginRight: 8 }}
              />
              Sign in with Google
            </Button>

            <Button
              fullWidth
              variant="outlined"
              sx={{ mt: 1, textTransform: "none" }}
            >
              <Facebook sx={{ mr: 1 }}></Facebook>
              Sign in with Facebook
            </Button>
          </Box>
          <Box>
            
          </Box>
        </Box>
      </Box>
    </Container>
  );
};

export default RegisterPage; 
