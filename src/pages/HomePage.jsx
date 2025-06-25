import { Box, Divider } from '@mui/material'
import React from 'react'
import RecentPosts from '../components/Post/RecentPosts';
import PostCategory from '../components/Post/PostCategory';

export default function HomePage() {
  return (
 
      <Box sx={{ display: "flex", height: "100vh" }}>
        <Box sx={{ flex: 3, bgcolor: "primary.main", p: 2 }}>
          <PostCategory/>
          <Divider sx={{mt: 1}}/>
        </Box>

        <Box sx={{ flex: 7, bgcolor: "secondary.main", p: 2 }}>
          <RecentPosts/>
          <Divider sx={{mt: 1}}/>
        </Box>
      </Box>
   
  );
}
