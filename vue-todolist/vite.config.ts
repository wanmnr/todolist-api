import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import AutoImport from 'unplugin-auto-import/vite';
import Components from 'unplugin-vue-components/vite';
import { ElementPlusResolver } from 'unplugin-vue-components/resolvers';

export default defineConfig({
  plugins: [
    vue(),
    // Auto-import Element Plus API like ElMessage
    AutoImport({
      resolvers: [ElementPlusResolver()],
      dts: 'src/auto-imports.d.ts', // Generates type declarations
      imports: ['vue'], // Import common utilities from Vue automatically
    }),
    // Auto-import Element Plus components like el-row, el-table
    Components({
      resolvers: [ElementPlusResolver()],
      dts: 'src/components.d.ts', // Generates type declarations
    }),
  ],
  server: {
    port: 3100,
  },
  build: {
    outDir: 'dist',
  },
});
