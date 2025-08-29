<script lang="ts">
    import { onMount } from "svelte";

    let funFact: string | null = null;
    let error: string | null = null; // Stores any error message that might occur
    let factUrl: string = ""; // Stores the URL of the meme image
    let isLoading: boolean = true;

    export let autoRefresh: boolean = true;
    export let refreshInterval: number = 360000;

    onMount(() => {
        loadFunfact();
        if (autoRefresh) {
            const intervalId = setInterval(loadFunfact, refreshInterval);

            // Clean up interval on component destruction
            return () => {
                clearInterval(intervalId);
            };
        }
    });
    async function loadFunfact() {
        try {
            const data = await fetchFunFact();
            console.log(data)
            if (data.url) {
                funFact = data.url;
                            console.log(funFact)

            } else if (data.error) {
                throw new Error(data.error);
            } else {
                throw new Error("Invalid response format");
            }
        } catch (err) {
            console.error("Failed to load fun fact:", err);
            error = err instanceof Error ? err.message : "Unknown error";
        } finally {
            isLoading = false;
        }
    }

    async function fetchFunFact() {
        const res = await fetch("/api/fetchFunFact");

        if (!res.ok) {
            throw new Error(`API returned ${res.status}`);
        }
        const data = await res.json();
        return data;
    }
</script>

<div class="funfact-container">
    {#if funFact}
        <p class="funfact">{funFact}</p>
    {:else if isLoading}
        <div class="loading-container">
            <p class="loading-text">Loading Fun Fact...</p>
            <div class="loading-spinner"></div>
        </div>
    {/if}
</div>

<style>
    .funfact-container {
        width: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 150px;
    }

    .funfact {
        font-family: "Muli", sans-serif;
        color: var(--secondary-sea-blue);
        text-wrap: wrap;
        font-size: 1.25rem;
        line-height: 1.5;
        max-width: 100%;
        overflow-wrap: break-word;
        margin: 0;
        text-align: center;
        padding: 1rem;
        background-color: rgba(204, 201, 175, 0.2);
        border-radius: 0.5rem;
        border-left: 4px solid var(--main-yellow);
    }

    @media (max-width: 768px) {
        .funfact {
            font-size: 1rem;
        }
    }

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
        to {
            transform: rotate(360deg);
        }
    }
</style>
