<script lang="ts">
    import { Meme } from "$lib";
    import { FunFact } from "$lib";
    import { Quote } from "$lib";
    import { Wyr } from "$lib";
    import { onMount } from "svelte";
    import Clock from "$lib/components/Clock.svelte";
    
    let currentView = 'all';
    let viewTimeout: ReturnType<typeof setTimeout>;
    

    function cycleViews() {
        if (currentView === 'all') {
            currentView = 'meme';
        } else if (currentView === 'meme') {
            currentView = 'facts';
        } else {
            currentView = 'all';
        }
        
        // Set the next view change
        if (viewTimeout) clearTimeout(viewTimeout);
        viewTimeout = setTimeout(cycleViews, 60000); // Change view every minute
    }

    onMount(() => {
        viewTimeout = setTimeout(cycleViews, 60000);
        
        return () => {
            if (viewTimeout) clearTimeout(viewTimeout);
        };
    });
</script>

<div class="page-wrapper view-{currentView}">
    <div class="header">
        <h1 class="main-title">Meme of the Day</h1>
        <div class="controls">
            <button class="view-button" on:click={() => currentView = 'all'} 
                   class:active={currentView === 'all'}>All Content</button>
            <button class="view-button" on:click={() => currentView = 'meme'} 
                   class:active={currentView === 'meme'}>Meme Only</button>
            <button class="view-button" on:click={() => currentView = 'facts'} 
                   class:active={currentView === 'facts'}>Facts Only</button>
        </div>
    </div>
    
    <Clock />

    <div class="container">
        <section class="card fact-section facts-view">
            <h2><span class="icon">💭</span> Quote</h2>
            <div class="card-content">
                <Quote />
            </div>
        </section>

        <section class="card meme-section meme-view">
            <h2><span class="icon">🖼️</span> Today's Meme</h2>
            <div class="card-content">
                <Meme />
            </div>
        </section>

        <section class="card fact-section facts-view">
            <h2><span class="icon">💡</span> Fun Fact</h2>
            <div class="card-content">
                <FunFact refreshInterval={30000} />
                <FunFact refreshInterval={45000} />
            </div>
        </section>

        <section class="card fact-section wyr-view">
            <h2><span class="icon">🤔</span> Would You Rather</h2>
            <div class="card-content">
                <Wyr />
            </div>
        </section>
    </div>
</div>

<style>
    .page-wrapper {
        margin: 0 auto;
        padding: 2rem 1rem;
        max-height: 100vh;
        max-width: 100vw;
        overflow: hidden;
        transition: background-color 0.5s ease;
    }

    .header {
        display: flex;
        justify-content: center;
        align-items: center;
        position: relative;
        margin-bottom: 2rem;
    }

    .main-title {
        text-align: center;
        font-size: 3.5rem;
        color: var(--secondary-sea-blue);
        text-transform: uppercase;
        letter-spacing: 1px;
        text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.1);
    }

    .container {
        display: flex;
        gap: 2rem;
        flex-direction: row;
        justify-content: space-evenly;
        flex-wrap: wrap;
        justify-items: space-evenly;
        transition: all 0.5s ease;
    }

    .card {
        background-color: var(--main-white);
        border-radius: 1rem;
        box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
        overflow: hidden;
        flex: 1;
        min-width: 20vw;
        max-width: 22vw;
        transition:
            transform 0.3s ease,
            box-shadow 0.3s ease;
        display: flex;
        flex-direction: column;
    }
    
    @keyframes fadeIn {
        from { opacity: 0; transform: translateY(10px); }
        to { opacity: 1; transform: translateY(0); }
    }
    
    .card {
        animation: fadeIn 0.8s ease-out;
    }

    h2 {
        text-align: center;
        padding: 1.5rem 1rem;
        margin: 0;
        background-color: var(--main-teal);
        color: var(--main-white);
        font-size: 1.5rem;
    }

    .card-content {
        padding: 1.5rem;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        flex-grow: 1;
        gap: 2rem;
    }

    /* View mode styles */
    .icon {
        margin-right: 0.5rem;
    }
    
    .controls {
        display: flex;
        gap: 1rem;
        position: absolute;
        right: 0;
        top: 50%;
        transform: translateY(-50%);
    }
    
    .view-button {
        background-color: var(--main-white);
        color: var(--main-teal);
        border: 2px solid var(--main-teal);
        padding: 0.5rem 1rem;
        border-radius: 0.5rem;
        cursor: pointer;
        font-weight: bold;
        transition: background-color 0.3s ease, transform 0.3s ease;
    }
    
    .view-button.active,
    .view-button:hover {
        background-color: var(--main-teal);
        color: var(--main-white);
        transform: translateY(-2px);
    }
    
    /* View mode display */
    .view-meme .facts-view {
        display: none;
    }
    
    .view-facts .meme-view {
        display: none;
    }
    
    .view-meme .wyr-view,
    .view-facts .wyr-view {
        display: none;
    }
    
    /* Add space between clock and meme in meme view */
    .view-meme :global(.clock-container) {
        margin-bottom: 3rem;
    }
    
    .view-meme .meme-view {
        max-width: 60vw;
        margin: 0 auto;
        transform: scale(1.1);
    }
    
    .view-facts .facts-view {
        width: 30vw;
        margin: 0 -500px;
    }
    
    @media (max-width: 768px) {
        .container {
            flex-direction: column;
            align-items: center;
        }

        .card {
            width: 100%;
            max-width: 100%;
        }

        .main-title {
            font-size: 2.25rem;
        }
        
        .controls {
            position: static;
            transform: none;
            flex-wrap: wrap;
            justify-content: center;
            margin-top: 1rem;
        }
    }
</style>
