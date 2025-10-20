(() => {
    const searchInput = document.querySelector('[data-snippet-search]');
    const grid = document.querySelector('[data-snippet-grid]');
    const emptyState = document.querySelector('[data-snippet-empty]');

    if (!searchInput || !grid) {
        return;
    }

    const items = Array.from(grid.querySelectorAll('[data-snippet-item]'));

    const applyFilter = () => {
        const query = searchInput.value.trim().toLowerCase();
        let visibleCount = 0;

        items.forEach((item) => {
            const text = (item.getAttribute('data-search-text') || '').toLowerCase();
            const isMatch = text.includes(query);
            item.style.display = isMatch ? '' : 'none';
            if (isMatch) {
                visibleCount += 1;
            }
        });

        if (emptyState) {
            emptyState.style.display = visibleCount === 0 ? 'block' : 'none';
        }
    };

    searchInput.addEventListener('input', applyFilter);

    applyFilter();
})();