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

<div class="app-container">
	<header>
		<div class="logo">
			<img src={favicon} alt="Logo" />
			<h1>Meme of the Day</h1>
		</div>
	</header>

	<main>
		{@render children?.()}
	</main>

	<footer>
		<div class="footer-content">
			<p>&copy; {currentYear} Meme of the Day. All rights reserved.</p>
			<p>Bringing smiles one meme at a time!</p>
		</div>
	</footer>
</div>

<style>
	.app-container {
		display: flex;
		flex-direction: column;
		min-height: 100vh;
	}

	header {
		background-color: var(--secondary-sea-blue);
		padding: 1rem;
		color: var(--main-white);
		box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
		border-radius: 1rem, 0;
	}

	.logo {
		display: flex;
		align-items: center;
		gap: 1rem;
	}

	.logo img {
		width: 40px;
		height: 40px;
	}

	.logo h1 {
		margin: 0;
		font-size: 1.5rem;
	}

	main {
		flex: 1;
		background-color: var(--main-beige);
	}

	footer {
		background-color: var(--secondary-sea-blue);
		color: var(--main-white);
		padding: 1.5rem;
		text-align: center;
		width: 100%;
		overflow-y: hidden;
	}

	.footer-content {
		width: 100%;
		margin: 0 auto;
	}

	footer p {
		margin: 0.5rem 0;
		font-size: 0.9rem;
	}
</style>
