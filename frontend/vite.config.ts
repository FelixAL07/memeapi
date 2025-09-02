import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';
import { resolve } from 'path';

export default defineConfig({
	plugins: [sveltekit()],
	server: {
		fs: {
			allow: [
				// Default allowed locations
				'.',
				// Add the static directory to the allowed list
				resolve(__dirname, 'static')
			]
		}
	}
});
