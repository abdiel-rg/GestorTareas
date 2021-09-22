export const showToast = (toastId) => {
    let reference = document.querySelector(`#${toastId}`);
    let toast = new bootstrap.Toast(reference);
    toast.show();
}

export const hideToast = (toastId) => {
    let reference = document.querySelector(`#${toastId}`);
    let toast = new bootstrap.Toast(reference);
    toast.hide();
}