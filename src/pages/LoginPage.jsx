import React, { useState } from 'react';
import { Box, TextField, Button, Typography, Container, Avatar, Link, Checkbox, FormControlLabel, Divider } from '@mui/material';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { CheckBox, Facebook, Google } from '@mui/icons-material';
import GoogleIcon from "../assets/icons/googleicon.png";
import apiService from '../services/apiSerivce';
import { useAppContext } from '../context/AppStore';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import Cookies from "js-cookie";
import { APIS } from '../constants/apis';
import { ROUTES } from '../constants/routes';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [error, setError] = useState('');
  const {setIsAuthenticated} = useAppContext();
  
  const navigate = useNavigate();
  
  const handleSubmit = async (event) => {
    event.preventDefault();
    // login logic
    const response = await apiService.post(APIS.Login, {userName: username, password: password});
    console.log('Login success:', response.data);
    if(response.status === 200){
      setIsAuthenticated(true);      
      Cookies.set("isLogged", true)
      const myprofile = await apiService.get(APIS.GetMyProfile, {})
      console.log("my profile after login: ", myprofile);
      sessionStorage.setItem("myProfile", JSON.stringify(myprofile.data));
      toast.success("Login successfully")
      window.location.href = ROUTES.HOME
    }
    
    console.log('Username:', username);
    console.log('Password:', password);
  };

  const test = async () => {
    const myprofile = await apiService.get(APIS.GetMyProfile, {});
    console.log(myprofile.data);
  }
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
          Sign In
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
            id="username"
            label="Username"
            name="username"
            autoComplete="username"
            autoFocus
            value={username}
            onChange={(e) => setUsername(e.target.value)}
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
          <Box sx={{ alignSelf: "flex-start" }}>
            <FormControlLabel
              control={
                <Checkbox
                  checked={rememberMe}
                  onChange={(e) => setRememberMe(e.target.checked)}
                  color="primary"
                />
              }
              label="Remember me"
              sx={{ justifyContent: "flex-start", ml: 0 }}
            />
          </Box>
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            Sign In
          </Button>
          <Link href="#" variant="body2">
            Forgot password?
          </Link>

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
              onClick={() => test()}
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
        </Box>
      </Box>
    </Container>
  );
};

export default LoginPage; 
