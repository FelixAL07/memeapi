<script lang="ts">
    import { onMount } from "svelte";

    let memeUrl: string = "";
    let isLoading: boolean = true;
    let error: string | null = null;

    // Fetch meme on component mount
    onMount(async () => {
        try {
            const response = await fetch("/api/fetchMeme");
            if (!response.ok) {
                throw new Error(`API returned ${response.status}`);
            }
            
            const data = await response.json();
            if (data.url) {
                memeUrl = data.url;
            } else if (data.error) {
                throw new Error(data.error);
            } else {
                throw new Error("Invalid response format");
            }
        } catch (err) {
            console.error("Failed to load meme:", err);
            error = err instanceof Error ? err.message : "Unknown error";
        } finally {
            isLoading = false;
        }
    });
</script>

<h1>Meme of the Day</h1>

{#if isLoading}
    <p>Loading meme...</p>
{:else if error}
    <p>Error: {error}</p>
{:else if memeUrl}
    <div class="meme-container">
        <img src={memeUrl} alt="Meme of the day" />
    </div>
{:else}
    <p>No meme available</p>
{/if}

<style>
    .meme-container {
        max-width: 600px;
        margin: 0 auto;
    }
    img {
        width: 100%;
        height: auto;
        border-radius: 8px;
    }
</style>
