<script lang="ts">
    import { onMount } from "svelte";

    let funFact: string | null = null;
    let error: string | null = null; // Stores any error message that might occur
    let isLoading: boolean = true;

    let charArray: Array<any> = [];

    export let autoRefresh: boolean = true;
    export let refreshInterval: number = 360000;

    onMount(() => {
        loadFunfact();

        let intervalId: ReturnType<typeof setInterval> | null = null;

        if (autoRefresh) {
            intervalId = setInterval(loadFunfact, refreshInterval);
        }

        return () => {
            if (intervalId) {
                clearInterval(intervalId);
            }
        };
    });

    async function loadFunfact() {
        try {
            const data = await fetchFunFact();

            if (data.text) {
                funFact = data.text;
                charArray = funFact ? [...funFact] : [];

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
        <p
            class="funfact"
            style="font-size: {charArray.length ? `calc(22vw / ${(charArray.length/3.5).toFixed(2)})` : '1.5rem'}"
        >
            {funFact}
        </p>
    {:else if isLoading}
        <div class="loading-container">
            <p class="loading-text">Loading Fun Fact...</p>
            <div class="loading-spinner"></div>
        </div>
    {:else if error}
        <p class="error-message">Error: {error}</p>
    {/if}
</div>

<style>
    .funfact-container {
        width: 100%;
        display: flex;
        justify-content: center;
        font-family: "Muli", sans-serif;
    }

    .funfact {
        font-family: "Muli", sans-serif;
        color: var(--secondary-sea-blue);
        text-wrap: wrap;
        line-height: 1.5;
        width: 100%;
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
    
    .error-message {
        color: #e74c3c;
        text-align: center;
        padding: 1rem;
        background-color: rgba(231, 76, 60, 0.1);
        border-radius: 0.5rem;
        border-left: 4px solid #e74c3c;
        width: 100%;
        margin: 0;
    }
</style>
