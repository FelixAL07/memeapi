<script lang="ts">
    import { onMount } from "svelte";

    let quote: string | null = null;
    let author: string | null = null;

    let error: string | null = null; // Stores any error message that might occur
    let isLoading: boolean = true;

    export let autoRefresh: boolean = true;
    export let refreshInterval: number = 60000;

    onMount(() => {
        loadQuote();
        if (autoRefresh) {
            const intervalId = setInterval(loadQuote, refreshInterval);

            // Clean up interval on component destruction
            return () => {
                clearInterval(intervalId);
            };
        }
    });
    async function loadQuote() {
        try {
            const data = await fetchQuote();
            console.log(data);
            if (data.quote) {
                quote = data.quote;
                console.log(quote);
            } else if (data.error) {
                throw new Error(data.error);
            } else {
                throw new Error("Invalid response format");
            }

            if (data.author) {
                author = data.author;
                console.log(author);
            } else if (data.error) {
                throw new Error(data.error);
            } else {
                throw new Error("Invalid response format");
            }
        } catch (err) {
            console.error("Failed to load quote:", err);
            error = err instanceof Error ? err.message : "Unknown error";
        } finally {
            isLoading = false;
        }
    }

    async function fetchQuote() {
        const res = await fetch("/api/fetchQuote");

        if (!res.ok) {
            throw new Error(`API returned ${res.status}`);
        }
        const data = await res.json();
        return data;
    }
</script>

<div class="funfact-container">
    {#if quote}
        <div>
            <p class="quote">"{quote}"</p>
            {#if author}
                <p class="author">- {author}</p>
            {/if}
        </div>
    {:else if isLoading}
        <div class="loading-container">
            <p class="loading-text">Loading Quote...</p>
            <div class="loading-spinner"></div>
        </div>
    {:else if error}
        <p class="error">Error: {error}</p>
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

    .quote {
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
        .quote {
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

    .author {
        font-family: "Oswald", sans-serif;
        font-style: italic;
        text-align: right;
        margin-top: 0.5rem;
        color: var(--secondary-black);
    }

    .error {
        color: #d32f2f;
        text-align: center;
        padding: 1rem;
        border-left: 4px solid #d32f2f;
        background-color: rgba(211, 47, 47, 0.1);
        border-radius: 0.5rem;
    }
</style>
