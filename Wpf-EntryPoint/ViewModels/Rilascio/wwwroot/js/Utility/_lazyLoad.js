export function initLazyLoad() {

    // Funzione per ottenere le dimensioni del viewport
    function getViewportSize() {
        return {
            width: window.innerWidth,
            height: window.innerHeight
        };
    }

    // Funzione per aggiornare gli attributi width e height delle immagini
    function updateImageDimensions() {

        console.log('ingresso updateImageDimensions')
        const viewportSize = getViewportSize();
        console.log('viewportSize', viewportSize)
        const images = document.querySelectorAll('.lazy-load-img');
        
        images.forEach(img => {

            console.log('img1', img)
            // Aggiorna gli attributi width e height
            img.setAttribute('width', viewportSize.width);
            img.setAttribute('height', viewportSize.height);

            console.log('img2', img)

        });
    }

    // Funzione per gestire il caricamento dell'elemento
    function lazyLoadElement(element, type) {
        const dataSrc = element.getAttribute('data-src');
        if (!dataSrc) return;

        element.src = dataSrc;
        element.removeAttribute('data-src');

        if (type === 'video') {
            element.load();
        }
    }

    // Funzione per gestire l'intersezione degli elementi
    function handleIntersection(entries, observer, type) {
        entries.forEach(entry => {
            const element = entry.target;
            if (entry.isIntersecting) {
                lazyLoadElement(element, type);
                observer.unobserve(element);
            }
        });
    }

    // Funzione per osservare gli elementi
    function observeElements(elements, type) {
        if (!elements || elements.length === 0) return;

        const observer = new IntersectionObserver((entries) => handleIntersection(entries, observer, type), { threshold: 0.5 });
        Array.from(elements).forEach(element => observer.observe(element));
    }

    // Aggiorna le dimensioni delle immagini e video
    updateImageDimensions();

    // Osserva gli elementi lazy load
    const images = document.getElementsByClassName('lazy-load-img');
    const videos = document.getElementsByClassName('thumbnail-video');

    observeElements(images, 'image');
    observeElements(videos, 'video');

    // Esegui l'aggiornamento delle dimensioni anche quando la finestra viene ridimensionata
    window.addEventListener('resize', updateImageDimensions);
}
