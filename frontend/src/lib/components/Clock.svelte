<script lang="ts">
    import { onMount, onDestroy } from "svelte";

    let time = new Date();
    let dateFormatted = formatDate(time);
    let intervalId: ReturnType<typeof setInterval>;

    onMount(() => {
        intervalId = setInterval(() => {
            time = new Date();
            dateFormatted = formatDate(time);
        }, 100);
    });

    onDestroy(() => {
        clearInterval(intervalId);
    });

    function formatDate(date: Date): string {
        const options: Intl.DateTimeFormatOptions = {
            weekday: "long",
            year: "numeric",
            month: "long",
            day: "numeric",
        };
        return date.toLocaleDateString("no-NO", options);
    }
</script>

<div class="clock-container">
    <div class="date">{dateFormatted}</div>
    <div class="time">{time.toLocaleTimeString("no-NO")}</div>
</div>

<style>
    .clock-container {
        text-align: center;
        padding: 1rem;
        border-radius: 0.5rem;
        background-color: rgba(255, 255, 255, 0.25);
        margin-bottom: 1rem;
    }


    .time {
        font-size: 2.5rem;
        font-weight: bold;
        color: var(--secondary-sea-blue);
        text-shadow: 5px 5px 5px rgba(0, 0, 0, 0.1);
    }

    .date {
        font-size: 1.25rem;
        color: var(--main-teal);
        margin-bottom: 0.5rem;
    }
</style>
