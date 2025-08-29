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
    <div class="loading-container">
        <p class="loading-text">Loading meme...</p>
        <div class="loading-spinner"></div>
    </div>
{:else if error}
    <div class="error-container">
        <p class="error-text">Error: {error}</p>
        <button on:click={loadMeme} class="retry-button">Try Again</button>
    </div>
{:else if memeUrl}
    <div class="meme-container">
        <img src={memeUrl} alt="Meme of the day" />
        <button on:click={loadMeme} class="refresh-button">
            <span class="refresh-icon">↻</span>
            <span class="refresh-text">New Meme</span>
        </button>
    </div>
{:else}
    <p class="no-meme">No meme available</p>
{/if}

<style>
    /* Container styles */
    .meme-container {
        width: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 1rem;
    }

    /* Image styles */
    img {
        width: 100%;
        max-width: 400px;
        height: auto;
        border-radius: 0.75rem;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        border: 3px solid var(--main-yellow);
        transition: transform 0.2s ease;
    }
    
    img:hover {
        transform: scale(1.02);
    }

    /* Loading styles */
    .loading-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 1rem;
        padding: 2rem;
    }
    
    .loading-text {
        color: var(--main-teal);
        font-style: italic;
    }
    
    .loading-spinner {
        width: 40px;
        height: 40px;
        border: 4px solid rgba(0, 139, 147, 0.2);
        border-left-color: var(--main-teal);
        border-radius: 50%;
        animation: spin 1s linear infinite;
    }
    
    @keyframes spin {
        to { transform: rotate(360deg); }
    }

    /* Error styles */
    .error-container {
        text-align: center;
        padding: 1.5rem;
        background-color: rgba(227, 82, 5, 0.1);
        border-radius: 0.5rem;
        border-left: 4px solid var(--secondary-orange);
    }
    
    .error-text {
        color: var(--secondary-orange);
        margin-bottom: 1rem;
    }

    /* Button styles */
    .retry-button, .refresh-button {
        background-color: var(--main-teal);
        color: var(--main-white);
        border: none;
        border-radius: 0.25rem;
        padding: 0.5rem 1rem;
        cursor: pointer;
        font-family: "Muli", sans-serif;
        transition: background-color 0.2s ease, transform 0.2s ease;
        display: flex;
        align-items: center;
        gap: 0.5rem;
    }
    
    .retry-button:hover, .refresh-button:hover {
        background-color: var(--secondary-sea-blue);
        transform: translateY(-2px);
    }
    
    .refresh-icon {
        font-size: 1.2rem;
        font-weight: bold;
    }
    
    .no-meme {
        color: var(--secondary-black);
        text-align: center;
        font-style: italic;
    }

    @media (max-width: 768px) {
        img {
            max-width: 100%;
        }
    }
</style>
