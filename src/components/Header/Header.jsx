import { AppBar, Avatar, Box, Button, Container, IconButton, Link, Menu, MenuItem, Toolbar, Tooltip, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import AllInclusiveIcon from '@mui/icons-material/AllInclusive';
import { useAppContext } from "../../context/AppStore";
import { useNavigate } from "react-router-dom";
import apiService from "../../services/apiSerivce";
const pages = ['Products', 'Pricing', 'Blog'];
const settings = ['Profile', 'Account', 'Dashboard', 'Logout'];

function Header() {

  const {isAuthenticated, setIsAuthenticated} = useAppContext();

  const [anchorElUser, setAnchorElUser] = useState(null);
  const navigate = useNavigate();

  const handleOpenUserMenu = (event) => {
    setAnchorElUser(event.currentTarget);
  };


  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const handleLogout = async () => {
    try {
      await apiService.post("/api/auth/logout"); 
  
      setIsAuthenticated(false);

      navigate("/auth/sign-in");
    } catch (error) {
      console.error("Error when logout:", error);
    }
  };


  return (
    <AppBar position="static">
       <Container
        sx={{
        maxWidth: {
          xs: '95%', 
          sm: '95%',  
          md: '80%',   
          lg: '80%',   
          xl: '80%',   
        },
      }}
    >
        <Toolbar disableGutters>
        <Box sx= {{flexGrow: 2, alignItems: "left"}}>
          <Link href="/" sx={{ display: 'flex', alignItems: 'center', textDecoration: 'none', color: 'white', maxWidth:"30vh"}}>
              <AllInclusiveIcon fontSize="large" sx={{ display: { md: 'flex' }, mr: 1, color: "white"}} />
              <Typography
                variant="h6"
                noWrap
                component="a"           
                sx={{
                  mr: 2,
                  display: 
                  {
                    xs: "none",
                    md: 'flex' 
                  },
                  fontFamily: 'monospace',
                  fontWeight: 700,
                  letterSpacing: '.3rem',
                  color: 'white',
                  textDecoration: 'none',
                  '&:hover': { color: "white" },
                }}
                
              >
                WEB BLOG
              </Typography>
            </Link>
            
        </Box>
        
        <Box sx={{ flexGrow: 1, display: "flex", justifyContent: "flex-end" }}>
          {!isAuthenticated ? 
          (<>
          <Button variant="text" style={{color: "white"}}
          onClick={() => navigate("/auth/sign-in")}
          >Login</Button>
          <Button variant="text"style={{color: "white"}}
          onClick={() => navigate("/auth/sign-up")}>Register</Button>
          </>
          ) : (
          <>
          <Tooltip title="Open settings">
            <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
              <Avatar alt="Remy Sharp" src="/static/images/avatar/2.jpg" />
            </IconButton>
          </Tooltip>
          <Menu
            sx={{ mt: '45px' }}
            id="menu-appbar"
            anchorEl={anchorElUser}
            anchorOrigin={{
              vertical: 'top',
              horizontal: 'right',
            }}
            keepMounted
            transformOrigin={{
              vertical: 'top',
              horizontal: 'right',
            }}
            open={Boolean(anchorElUser)}
            onClose={handleCloseUserMenu}
          >
            {settings.map((setting) => (
              <MenuItem key={setting} onClick={setting === "Logout" ? handleLogout : handleCloseUserMenu}>
                <Typography sx={{ textAlign: 'center' }}>{setting}</Typography>
              </MenuItem>
            ))}
          </Menu>
          </>
        )}
          
          
        </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
}
export default Header;
