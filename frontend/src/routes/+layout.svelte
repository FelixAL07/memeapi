<script lang="ts">
	import favicon from "$lib/assets/favicon.svg";
	import "../index.css";

	let { children } = $props();
	const currentYear = new Date().getFullYear();
</script>

<svelte:head>
	<link rel="icon" href={favicon} />
	<title>Meme of the Day</title>
</svelte:head>
	<main>
		{@render children?.()}
	</main>