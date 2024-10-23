export function initReels() {
    // Selettori principali
    const videoFeed = document.querySelector('.video-feed');
    const videoContainers = document.querySelectorAll('.video-container');
    const videos = document.querySelectorAll('.video-item');
    const commentsOverlay = document.getElementById('commentsOverlay');
    const sendCommentButton = document.getElementById('sendComment');
    const commentInput = document.getElementById('commentInput');
    const commentDisplay = document.getElementById('commentsDisplay');

    if (!videoFeed || !videoContainers.length || !videos.length) {
        console.warn('Elementi principali mancanti. Assicurati che video-feed, video-container e video-item siano presenti.');
        return;
    }

    // Funzione per riprodurre il video
    const playVideo = (video) => video.play();

    // Funzione per mettere in pausa il video
    const pauseVideo = (video) => video.pause();

    // Funzione per gestire lo scorrimento
    const handleScroll = () => {
        videos.forEach(video => {
            const rect = video.getBoundingClientRect();
            if (rect.top >= 0 && rect.bottom <= window.innerHeight) {
                playVideo(video);
            } else {
                pauseVideo(video);
            }
        });
    };

    // Aggiungi evento di scorrimento per la riproduzione/pausa dei video
    videoFeed.addEventListener('scroll', handleScroll);

    // Controllo iniziale dello stato dei video
    handleScroll();

    // Gestione dei commenti
    document.querySelectorAll('.comment-button').forEach(button => {
        button.addEventListener('click', async function () {
            if (!commentsOverlay) return;

            commentsOverlay.style.display = 'flex';

            const reelsId = this.previousElementSibling.value;

            // Chiamata asincrona al controller per ottenere i commenti
            const comments = await fetchComments(reelsId);

            // Carica i commenti nell'overlay
            loadComments(comments);
        });
    });

    // Bottone per inviare un commento
    if (sendCommentButton && commentInput && commentDisplay) {
        sendCommentButton.addEventListener('click', function () {
            const commentText = commentInput.value.trim();

            if (commentText !== '') {
                // Crea un nuovo elemento per il commento
                const newComment = document.createElement('div');
                newComment.textContent = commentText;
                newComment.className = 'comment';

                // Aggiungi il nuovo commento alla lista dei commenti
                commentDisplay.appendChild(newComment);

                // Svuota l'input del commento
                commentInput.value = '';
            }
        });
    }

    resizeReels();

    // Scorrimento manuale al primo video
    const firstVideoContainer = document.querySelector('.video-container');
    if (firstVideoContainer) {
        videoFeed.scrollTop = firstVideoContainer.offsetTop;
    }
}

function resizeReels() {
    // Selettori principali
    const containerScreen = document.querySelector('.video-feed');
    const divs = document.querySelectorAll('.video-container');

    if (!containerScreen || !divs.length) {
        console.warn('Elementi principali mancanti per resizeReels.');
        return;
    }

    const containerWidth = window.innerWidth;
    const containerHeight = window.innerHeight;

    const videoWidthPercentage = 0.25; // 25% della larghezza del container
    const videoHeightPercentage = 0.80; // 80% dell'altezza del container

    let progressivoTop = 0;

    divs.forEach(div => {
        const videoWidth = containerWidth * videoWidthPercentage;
        const videoHeight = containerHeight * videoHeightPercentage;

        let top = ((containerHeight - videoHeight) / 2) + progressivoTop;
        const left = (containerWidth - videoWidth) / 2;

        if (containerHeight < 700 || containerWidth < 500) {
            top = containerHeight + progressivoTop;

            div.style.width = '100%';
            div.style.height = '100%';
            div.style.top = `${top}px`;

            containerScreen.style.height = '100%';
            containerScreen.style.top = '0%';

            progressivoTop += top;

            const videoItem = div.querySelector('.video-item');
            const utilityBarReels = div.querySelector('.utility-bar-reels');
            const utilityBarTxt = div.querySelector('.utility-bar-txt');
            const utilityBarBtn = div.querySelector('.utility-bar-btn');

            if (videoItem) {
                videoItem.style.width = '100%';
                videoItem.style.position = 'absolute';
                videoItem.style.borderRadius = '0px';
            }

            if (utilityBarReels) {
                utilityBarReels.style.position = 'absolute';
                utilityBarReels.style.width = '100%';
                utilityBarReels.style.height = '30%';
                utilityBarReels.style.bottom = '5%';
            }

            if (utilityBarBtn) {
                utilityBarBtn.style.display = 'flex';
                utilityBarBtn.style.flexDirection = 'row';
                utilityBarBtn.style.justifyContent = 'flex-start';
                utilityBarBtn.style.gap = '30px';

                const icons = utilityBarBtn.querySelectorAll('i');
                icons.forEach(icon => {
                    icon.classList.remove('fa-4x');
                    icon.classList.add('fa-2x');
                });
            }

            if (utilityBarTxt) {
                utilityBarTxt.style.fontSize = '30px';
            }
        } else {
            div.style.width = `${videoWidth}px`;
            div.style.height = `${videoHeight}px`;
            div.style.top = `${top}px`;
            div.style.left = `${left}px`;

            progressivoTop += (top * 2 + videoHeight);
        }
    });
}

async function fetchComments(reelsId) {
    try {
        const response = await fetch(`/Reels/GetComments?id_reels=${reelsId}`);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return await response.json();
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        return [];
    }
}

function loadComments(comments) {
    const commentDisplay = document.getElementById('commentsDisplay');
    if (!commentDisplay) return;

    commentDisplay.innerHTML = ''; // Svuota i commenti esistenti
    comments.forEach(comment => {
        const commentElement = document.createElement('div');
        commentElement.textContent = comment.text; // Assumendo che il commento abbia una proprietà 'text'
        commentElement.className = 'comment';
        commentDisplay.appendChild(commentElement);
    });
}
