<script lang="ts">
    import { onMount } from "svelte";

    // Props that can be passed to this component
    export let autoRefresh: boolean = true;
    export let refreshInterval: number = 360000; // 1 hour in milliseconds

    let memeUrl: string = ""; // Stores the URL of the meme image
    let isLoading: boolean = true; // Tracks loading state of the meme
    let error: string | null = null; // Stores any error message that might occur

    // Fetch meme data when component is mounted to the DOM
    onMount(() => {
        // Fetch meme immediately on mount
        loadMeme();

        // Set up interval for auto-refresh if enabled
        if (autoRefresh) {
            const intervalId = setInterval(loadMeme, refreshInterval);

            // Clean up interval on component destruction
            return () => {
                clearInterval(intervalId);
            };
        }
    });

    // Function to load the meme
    async function loadMeme() {
        isLoading = true; // For conditional rendering
        try {
            const data = await fetchMeme(); 
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
    }

    //Function to fetch the meme
    async function fetchMeme() {
        const response = await fetch("/api/fetchMeme");

        // Check if the response is successful
        if (!response.ok) {
            throw new Error(`API returned ${response.status}`);
        }

        const data = await response.json();
        return data;
    }
</script>

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
    /* Container to center and limit the width of the meme image */
    .meme-container {
        max-width: 600px;
        margin: 0 auto;
    }

    /* Style for the meme image */
    img {
        width: 100%;
        height: auto;
        border-radius: 1rem; /* Rounded corners for the image */
        border: 5px solid var(--main-yellow); /* Combines width, style, and color in one property */
    }
</style>
