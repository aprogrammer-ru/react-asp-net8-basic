import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path';

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  css: {
    preprocessorOptions: {
      scss: {
        additionalData: `@import "~bootstrap/scss/bootstrap";`,
      },
    },
  },
  build: {
    outDir: path.resolve(__dirname, '../AspApiBasic/ClientApp'), // Указываем целевую директорию
    emptyOutDir: true, // Очищать целевую директорию перед сборкой
  },
  server: {
    port: 5272, // Порт для разработки
  },
})
