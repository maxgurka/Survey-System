// Hides doughnut charts by default
const elements = $('[id^="' + "doughnut_" + '"]');
elements.hide();

updateMasonryLayout();

// Toggle between doughnut and bar charts
function toggleElementsStartingWith(namePrefix, enabled) {
    const elements = $('[id^="' + namePrefix + '"]');
    elements.each(function () {
        const element = $(this);
        if (enabled) {
            element.show(500, function () {
                clearInterval(animationInterval);
            });
        } else {
            element.hide(500, function () {
                clearInterval(animationInterval);
            });
        }
        var animationInterval = setInterval(function () {
            updateMasonryLayout(); //Update the masonry layout during the animation so the container size changes smoothly
        }, 25);
    });
}

// Updates the masonry to make sure it tiles correctly on changes
function updateMasonryLayout() {
    var masonryContainer = document.getElementById('masonry-container');
    var masonry = new Masonry(masonryContainer, {
        itemSelector: '.masonry-item',
        columnWidth: '.masonry-item',
        percentPosition: true
    });

    // Layout Masonry items
    masonry.layout();
}

// Buttons for toggling between bar and doughnut charts
$('#enable-bar-chart').on('click', function () {
    toggleElementsStartingWith('bar_', true);
    toggleElementsStartingWith('doughnut_', false);

    // Toggle active class and icon styles
    $('#enable-bar-chart').addClass('active');
    $('#enable-doughnut-chart').removeClass('active');

    // Remove focus to prevent highlighting
    $('#enable-bar-chart').blur();
});

$('#enable-doughnut-chart').on('click', function () {
    toggleElementsStartingWith('bar_', false);
    toggleElementsStartingWith('doughnut_', true);

    // Toggle active class and icon styles
    $('#enable-doughnut-chart').addClass('active');
    $('#enable-bar-chart').removeClass('active');

    // Remove focus to prevent highlighting
    $('#enable-doughnut-chart').blur();
});

// Switch arrow icon when expanding/collapsing
$(".collapseButton").on("click", function () {
    $(this).find(".bi").toggleClass("bi-chevron-up bi-chevron-down");
});