module.exports = {
  apps: [
    {
      name: 'nestjs',
      script: 'dist/main.js', // Path to compiled NestJS entry file
      instances: 8, // Use a number, not a string, for instances
      exec_mode: 'cluster',   // Enable cluster mode
      env: {
        NODE_ENV: 'production',
      },
    },
  ],
};