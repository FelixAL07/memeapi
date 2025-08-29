// place files you want to import through the `$lib` alias in this folder.
// Note: This file re-exports components to make them easier to import in other parts of the application
// You can now use: import { Meme } from '$lib' instead of import Meme from '$lib/components/meme.svelte'

// Re-export the components
// The @ts-ignore comment is needed because TypeScript doesn't recognize .svelte files by default
// @ts-ignore
export { default as Meme } from './components/meme.svelte';
export { default as FunFact } from './components/funFact.svelte';
