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
        }
    })
})