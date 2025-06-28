// Import SignalR
//const signalR = require('@microsoft/signalr');

// SignalR Connection Setup
var newNotificationConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/taskNotification")
    .withAutomaticReconnect([0, 2000, 10000, 30000]) // Auto-reconnect with delays
    .build();

// Helper Functions
function getNotificationIcon(details) {
    // إضافة فحص للتأكد من وجود القيمة
    if (!details || typeof details !== 'string') {
        return 'bi bi-info-circle-fill';
    }

    const detailsLower = details.toLowerCase();
    if (detailsLower.includes('team')) return 'bi bi-people-fill';
    if (detailsLower.includes('invitation')) return 'bi bi-envelope-fill';
    if (detailsLower.includes('task')) return 'bi bi-check-square-fill';
    if (detailsLower.includes('workspace')) return 'bi bi-kanban-fill';
    return 'bi bi-info-circle-fill';
}

function getTimeAgo(dateTime) {
    try {
        const now = new Date();
        const notificationDate = new Date(dateTime);
        const diffInMinutes = Math.floor((now - notificationDate) / (1000 * 60));

        if (diffInMinutes < 1) return 'Just now';
        if (diffInMinutes < 60) return `${diffInMinutes}m ago`;

        const diffInHours = Math.floor(diffInMinutes / 60);
        if (diffInHours < 24) return `${diffInHours}h ago`;

        const diffInDays = Math.floor(diffInHours / 24);
        if (diffInDays < 7) return `${diffInDays}d ago`;

        return notificationDate.toLocaleDateString('en-US', {
            month: 'short',
            day: 'numeric'
        });
    } catch (error) {
        console.error('Error formatting time:', error);
        return 'Unknown time';
    }
}

function formatDateTime(dateTime) {
    try {
        const date = new Date(dateTime);
        return date.toLocaleDateString('en-US', {
            month: 'short',
            day: 'numeric',
            year: 'numeric',
            hour: 'numeric',
            minute: '2-digit',
            hour12: true
        });
    } catch (error) {
        console.error('Error formatting date:', error);
        return 'Unknown date';
    }
}

function updateUnreadCount(increment = true) {
    const unreadCountElement = document.getElementById('unread-count');
    if (unreadCountElement) {
        const currentCount = parseInt(unreadCountElement.textContent) || 0;
        const newCount = increment ? currentCount + 1 : Math.max(0, currentCount - 1);
        unreadCountElement.textContent = newCount;

        // Update navbar notification badge if exists
        const navBadge = document.querySelector('.notification-badge');
        if (navBadge) {
            navBadge.textContent = newCount;
            navBadge.style.display = newCount > 0 ? 'block' : 'none';
        }
    }
}

function showNotificationToast(notification) {
    // التأكد من وجود البيانات المطلوبة
    if (!notification || !notification.details) {
        console.error('Invalid notification data:', notification);
        return;
    }

    // Create toast notification for immediate feedback
    const toast = document.createElement('div');
    toast.className = 'notification-toast';
    toast.innerHTML = `
        <div class="toast-content">
            <div class="toast-icon">
                <i class="${getNotificationIcon(notification.details)}"></i>
            </div>
            <div class="toast-body">
                <div class="toast-title">New Notification</div>
                <div class="toast-message">${escapeHtml(notification.details)}</div>
            </div>
            <button class="toast-close" onclick="this.parentElement.remove()">
                <i class="bi bi-x"></i>
            </button>
        </div>
    `;

    // Add toast styles if not already added
    if (!document.querySelector('#toast-styles')) {
        const style = document.createElement('style');
        style.id = 'toast-styles';
        style.textContent = `
            .notification-toast {
                position: fixed;
                top: 80px;
                right: 20px;
                background: white;
                border-radius: 12px;
                box-shadow: 0 10px 25px rgba(0,0,0,0.15);
                border-left: 4px solid #6366f1;
                z-index: 9999;
                animation: slideInRight 0.3s ease-out;
                max-width: 400px;
                min-width: 300px;
            }
            .toast-content {
                display: flex;
                align-items: center;
                padding: 1rem;
                gap: 0.75rem;
            }
            .toast-icon {
                width: 40px;
                height: 40px;
                background: rgba(99, 102, 241, 0.1);
                border-radius: 50%;
                display: flex;
                align-items: center;
                justify-content: center;
                color: #6366f1;
                font-size: 1.25rem;
                flex-shrink: 0;
            }
            .toast-body {
                flex: 1;
            }
            .toast-title {
                font-weight: 600;
                color: #1f2937;
                margin-bottom: 0.25rem;
            }
            .toast-message {
                color: #6b7280;
                font-size: 0.875rem;
                line-height: 1.4;
            }
            .toast-close {
                background: none;
                border: none;
                color: #9ca3af;
                cursor: pointer;
                padding: 0.25rem;
                border-radius: 4px;
                transition: all 0.2s;
            }
            .toast-close:hover {
                background: #f3f4f6;
                color: #6b7280;
            }
            @keyframes slideInRight {
                from {
                    transform: translateX(100%);
                    opacity: 0;
                }
                to {
                    transform: translateX(0);
                    opacity: 1;
                }
            }
        `;
        document.head.appendChild(style);
    }

    document.body.appendChild(toast);

    // Auto remove after 5 seconds
    setTimeout(() => {
        if (toast.parentElement) {
            toast.style.animation = 'slideInRight 0.3s ease-out reverse';
            setTimeout(() => toast.remove(), 300);
        }
    }, 5000);
}

function addNotificationToList(notification) {
    const notificationsList = document.querySelector('.notifications-list');
    if (!notificationsList || !notification) return;

    const notificationElement = document.createElement('div');
    notificationElement.className = 'notification-card unread new-notification';
    notificationElement.setAttribute('data-notification-id', notification.id);

    notificationElement.innerHTML = `
        <div class="notification-indicator">
            <div class="unread-dot"></div>
        </div>
        <div class="notification-content">
            <div class="notification-icon">
                <i class="${getNotificationIcon(notification.details)}"></i>
            </div>
            <div class="notification-body">
                <div class="notification-message font-bold">
                    ${escapeHtml(notification.details)}
                </div>
                <div class="notification-meta">
                    <span class="notification-time">
                        <i class="bi bi-clock"></i>
                        ${getTimeAgo(notification.dateToSend)}
                    </span>
                    <span class="notification-date">
                        ${formatDateTime(notification.dateToSend)}
                    </span>
                </div>
            </div>
            <div class="notification-actions">
                <a href="/Notification/MakeRead?Id=${notification.id}" 
                   class="btn btn-mark-read mark-as-read-link">
                    <i class="bi bi-check"></i>
                    <span>Mark Read</span>
                </a>
            </div>
        </div>
    `;

    // Add to top of list
    notificationsList.insertBefore(notificationElement, notificationsList.firstChild);

    // Add entrance animation
    setTimeout(() => {
        notificationElement.classList.add('animate-in');
    }, 100);

    // Add click handler for mark as read
    const markReadLink = notificationElement.querySelector('.mark-as-read-link');
    if (markReadLink) {
        markReadLink.addEventListener('click', function (e) {
            e.preventDefault();
            markNotificationAsRead(this, notification.id);
        });
    }
}

function markNotificationAsRead(element, notificationId) {
    const notificationCard = element.closest('.notification-card');

    fetch(`/Notification/MakeRead?Id=${notificationId}`, {
        method: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
        .then(response => {
            if (response.ok) {
                // Update UI
                notificationCard.classList.remove('unread');
                notificationCard.classList.add('read');

                // Update indicator
                const indicator = notificationCard.querySelector('.unread-dot');
                if (indicator) {
                    indicator.className = 'read-dot';
                }

                // Update message styling
                const message = notificationCard.querySelector('.notification-message');
                if (message) {
                    message.classList.remove('font-bold');
                }

                // Update action button
                element.outerHTML = `
                <div class="read-status">
                    <i class="bi bi-check-circle-fill"></i>
                    <span>Read</span>
                </div>
            `;

                // Update counter
                updateUnreadCount(false);

                // Add success animation
                notificationCard.style.transform = 'scale(0.98)';
                setTimeout(() => {
                    notificationCard.style.transform = 'scale(1)';
                }, 150);
            }
        })
        .catch(error => {
            console.error('Error marking notification as read:', error);
            showErrorToast('Failed to mark notification as read');
        });
}

function showErrorToast(message) {
    const toast = document.createElement('div');
    toast.className = 'notification-toast error-toast';
    toast.innerHTML = `
        <div class="toast-content">
            <div class="toast-icon error">
                <i class="bi bi-exclamation-triangle-fill"></i>
            </div>
            <div class="toast-body">
                <div class="toast-title">Error</div>
                <div class="toast-message">${escapeHtml(message)}</div>
            </div>
            <button class="toast-close" onclick="this.parentElement.remove()">
                <i class="bi bi-x"></i>
            </button>
        </div>
    `;

    document.body.appendChild(toast);
    setTimeout(() => toast.remove(), 3000);
}

// Helper function لحماية من XSS
function escapeHtml(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// SignalR Event Handlers
newNotificationConnection.on("ReceiveNotification", (notification) => {
    console.log("New notification received:", notification);

    try {
        // التحقق من صحة البيانات
        if (!notification) {
            console.error('Notification is null or undefined');
            return;
        }

        // طباعة البيانات للتأكد من البنية
        console.log('Notification structure:', {
            id: notification.id,
            details: notification.details,
            dateToSend: notification.dateToSend,
            isRead: notification.isRead
        });

        // Show toast notification
        showNotificationToast(notification);

        // Add to notifications list if on notifications page
        addNotificationToList(notification);

        // Update unread count
        updateUnreadCount(true);

        // Play notification sound (optional)
        playNotificationSound();

        console.log("Notification processed successfully");
    } catch (error) {
        console.error("Error processing notification:", error);
        console.error("Notification data:", notification);
    }
});

// Connection event handlers
newNotificationConnection.onclose((error) => {
    console.error("SignalR connection closed:", error);
    showErrorToast("Connection lost. Trying to reconnect...");
});

newNotificationConnection.onreconnecting((error) => {
    console.log("SignalR reconnecting:", error);
});

newNotificationConnection.onreconnected((connectionId) => {
    console.log("SignalR reconnected:", connectionId);
});

// Optional: Play notification sound
function playNotificationSound() {
    try {
        // Create audio element for notification sound
        const audio = new Audio('data:audio/wav;base64,UklGRnoGAABXQVZFZm10IBAAAAABAAEAQB8AAEAfAAABAAgAZGF0YQoGAACBhYqFbF1fdJivrJBhNjVgodDbq2EcBj+a2/LDciUFLIHO8tiJNwgZaLvt559NEAxQp+PwtmMcBjiR1/LMeSwFJHfH8N2QQAoUXrTp66hVFApGn+DyvmwhBSuBzvLZiTYIG2m98OScTgwOUarm7blmGgU7k9n1unEiBC13yO/eizEIHWq+8+OWT');
        audio.volume = 0.3;
        audio.play().catch(e => console.log("Could not play notification sound:", e));
    } catch (error) {
        console.log("Notification sound not available:", error);
    }
}

// Connection management
function connectionSuccess() {
    console.log("SignalR connection established successfully.");
}

function connectionFailed(error) {
    console.error("SignalR connection failed:", error);
    showErrorToast("Failed to connect to notification service");
}

// Initialize connection
console.log("Initializing SignalR connection...");
newNotificationConnection.start()
    .then(connectionSuccess)
    .catch(connectionFailed);

// Add CSS for animations
const animationStyles = document.createElement('style');
animationStyles.textContent = `
    .new-notification {
        transform: translateX(100%);
        opacity: 0;
        transition: all 0.5s ease-out;
    }
    
    .new-notification.animate-in {
        transform: translateX(0);
        opacity: 1;
    }
    
    .error-toast {
        border-left-color: #ef4444 !important;
    }
    
    .error-toast .toast-icon {
        background: rgba(239, 68, 68, 0.1) !important;
        color: #ef4444 !important;
    }
`;
document.head.appendChild(animationStyles);

// Export for global access if needed
window.NotificationManager = {
    markAsRead: markNotificationAsRead,
    updateCount: updateUnreadCount,
    showToast: showNotificationToast
};