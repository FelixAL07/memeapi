<script lang="ts">
    import { onMount } from "svelte";

    let before: string | null = null;
    let after: string | null = null;

    let error: string | null = null;
    let isLoading: boolean = true;

    export let autoRefresh: boolean = true;
    export let refreshInterval: number = 360000;

    onMount(() => {
        loadWYR();

        let intervalId: ReturnType<typeof setInterval> | null = null;

        if (autoRefresh) {
            intervalId = setInterval(loadWYR, refreshInterval);
        }

        return () => {
            if (intervalId) {
                clearInterval(intervalId);
            }
        };
    });

    async function loadWYR() {
        try {
            const data = await fetchWYR();
            if (data.before && data.after) {
                before = data.before;
                after = data.after;
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

    async function fetchWYR() {
        const res = await fetch("/api/fetchWYR");

        if (!res.ok) {
            throw new Error(`API returned ${res.status}`);
        }
        const data = await res.json();
        return data;
    }
</script>

<div class="WYR-container">
    {#if before && after}
        <div class="WYR-option before">{before}</div>
        <div class="or-divider">
            <span>OR</span>
        </div>
        <div class="WYR-option after">{after}</div>
    {:else if isLoading}
        <div class="loading-container">
            <p class="loading-text">Loading Would You Rather...</p>
            <div class="loading-spinner"></div>
        </div>
    {/if}
</div>

<style>
    .WYR-container {
        width: 100%;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: stretch;
        min-height: 180px;
        gap: 1rem;
        text-align: center;
    }

    .WYR-option {
        font-family: "Muli", sans-serif;
        font-size: 1.25rem;
        line-height: 1.6;
        color: var(--secondary-sea-blue);
        padding: 1.25rem;
        border-radius: 0.75rem;
        background: rgba(204, 201, 175, 0.15);
        border: 2px solid transparent;
        transition: all 0.3s ease;
        cursor: pointer;
    }

    /* Hover effect makes it feel interactive */
    .WYR-option:hover {
        background: rgba(255, 223, 80, 0.25);
        border-color: var(--main-yellow);
        transform: scale(1.02);
    }

    /* Differentiate before/after with subtle accents */
    .WYR-option.before {
        border-left: 5px solid var(--main-yellow);
    }

    .WYR-option.after {
        border-right: 5px solid var(--main-teal);
    }

    .or-divider {
        position: relative;
        display: flex;
        align-items: center;
        justify-content: center;
        color: var(--main-teal);
        font-weight: bold;
        font-size: 0.9rem;
        text-transform: uppercase;
        letter-spacing: 1px;
    }

    .or-divider::before,
    .or-divider::after {
        content: "";
        flex: 1;
        height: 1px;
        background: rgba(0, 139, 147, 0.25);
        margin: 0 0.75rem;
    }

    @media (max-width: 768px) {
        .WYR-option {
            font-size: 1rem;
            padding: 1rem;
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

