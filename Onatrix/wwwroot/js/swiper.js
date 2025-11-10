document.addEventListener('DOMContentLoaded', function () {
    const swiper = new Swiper('.serviceSwiper', {
        slidesPerView: 1.2,
        spaceBetween: 16,
        centeredSlides: true,
        centeredSlidesBounds: true,
        slidesOffsetBefore: 16,
        slidesOffsetAfter: 16,
        
        pagination: {
            el: '.swiper-pagination',
            clickable: true,
            renderBullet: (i, className) => `<span class="${className}">${i + 1}</span>`
        },
        
        navigation: {
            nextEl: '.custom-next',
            prevEl: '.custom-prev',
        },
        
        breakpoints: {
            768: {
                slidesPerView: 2,
                grid: { rows: 2, fill: 'row' },
                slidesPerGroup: 2,
                centeredSlides: false,
                centeredSlidesBounds: false,
                slidesOffsetBefore: 0,
                slidesOffsetAfter: 0,
            },
            
            1024: {
                slidesPerView: 3,
                grid: { rows: 2, fill: 'row' },
                slidesPerGroup: 2,
                centeredSlides: false,
                centeredSlidesBounds: false,
                slidesOffsetBefore: 0,
                slidesOffsetAfter: 0,
            }
        }
    })
})