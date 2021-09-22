export const toggleModal = (modalID) => {
    let reference = document.querySelector(`#${modalID}`)
    let modal = new bootstrap.Modal(reference);
    modal.toggle();
}