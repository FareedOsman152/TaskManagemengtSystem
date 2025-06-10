var newNotificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/taskNotification")
    .build();

newNotificationConnection.on("ReceiveNewNotification", (id, details, isRead, taskId, date) => {
    console.log("Event camed in real time")
    // 1. إنشاء عنصر HTML جديد للإشعار
    const notificationElement = `
        <div class="list-group-item list-group-item-action flex-column align-items-start list-group-item-primary"
             data-notification-id="${id}">
            <div class="d-flex w-100 justify-content-between">
                <div class="mb-1">
                    <p class="mb-1 fw-bold">${details}</p>
                    <small class="text-muted">
                        <i class="far fa-clock me-1"></i>
                        ${new Date(date).toLocaleString()}
                    </small>
                </div>
                <div>
                    <a href="/Notification/MakeRead?id=${id}"
                       class="btn btn-sm btn-outline-primary mark-as-read">
                        <i class="fas fa-check"></i> Make Read
                    </a>
                </div>
            </div>
        </div>
    `;

    // 2. إضافة الإشعار الجديد في أعلى القائمة
    const notificationsList = document.querySelector('.list-group');
    notificationsList.insertAdjacentHTML('afterbegin', notificationElement);

    // 3. تحديث عداد الإشعارات غير المقروءة
    const unreadCountElement = document.getElementById('unread-count');
    const currentCount = parseInt(unreadCountElement.textContent) || 0;
    unreadCountElement.textContent = (currentCount + 1) + ' unread';

    // 4. إضافة حدث النقر لزر "Make Read"
    const newReadButton = notificationsList.querySelector('.mark-as-read');
    if (newReadButton) {
        newReadButton.addEventListener('click', function (e) {
            e.preventDefault();
            const notificationItem = this.closest('.list-group-item');

            fetch(this.href, { method: 'POST' })
                .then(() => {
                    notificationItem.classList.remove('list-group-item-primary');
                    notificationItem.querySelector('p').classList.remove('fw-bold');
                    this.outerHTML = '<span class="badge bg-success">Read</span>';

                    // تحديث العداد
                    unreadCountElement.textContent = (currentCount) + ' unread';
                });
        });
    }
});

console.log("file is here");
function connectionSucess() {
    console.log("SignalR connection Sucess.");
}

function connectionFaild() {
    console.log("SignalR connection Error.");
}

newNotificationConnection.start().then(connectionSucess, connectionFaild);

