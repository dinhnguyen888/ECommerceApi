import app from './app';

// Start server
const PORT = process.env.PORT || 3005;
app.listen(PORT, () => {
  console.log(`Notification Service running on port ${PORT}`);
});