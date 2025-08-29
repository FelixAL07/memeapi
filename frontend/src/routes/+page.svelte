<script lang="ts">
    import { Meme } from "$lib";
    import {FunFact} from "$lib";
    import {Quote} from "$lib";
</script>

<div class="page-wrapper">
    <h1 class="main-title">Meme of the Day</h1>

    <div class="container">
        <section class="card fact-section">
            <h2>Quote</h2>
            <div class="card-content">
                <Quote />
            </div>
        </section>

        <section class="card meme-section">
            <h2>Today's Meme</h2>
            <div class="card-content">
                <Meme />
            </div>
        </section>

        <section class="card fact-section">
            <h2>Fun Fact</h2>
            <div class="card-content">
                <FunFact />
                <FunFact />
            </div>
        </section>
    </div>
</div>

<style>
    .page-wrapper {
        margin: 0 auto;
        padding: 2rem 1rem;
    }

    .main-title {
        text-align: center;
        font-size: 3rem;
        color: var(--secondary-sea-blue);
        margin-bottom: 2rem;
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
        
    }

    .card {
        background-color: var(--main-white);
        border-radius: 1rem;
        box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
        overflow: hidden;
        flex: 1;
        min-width: 300px;
        max-width: 500px;
        transition:
            transform 0.3s ease,
            box-shadow 0.3s ease;
        display: flex;
        flex-direction: column;
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
    }

    /* Responsive adjustments */
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
    }
</style>
