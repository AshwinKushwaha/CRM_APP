// CRMApp - Client-side utilities
(function () {
    'use strict';

    // Utility functions for CRM application
    const CRMUtils = {
        
        // Add smooth scroll behavior to anchor links
        initSmoothScroll: function () {
            const anchors = document.querySelectorAll('a[href^="#"]');
            if (!anchors || anchors.length === 0) return;
            
            anchors.forEach(anchor => {
                if (!anchor) return;
                
                anchor.addEventListener('click', function (e) {
                    const href = this.getAttribute('href');
                    if (!href || href === '#') return;
                    
                    const target = document.querySelector(href);
                    if (target) {
                        e.preventDefault();
                        target.scrollIntoView({
                            behavior: 'smooth',
                            block: 'start'
                        });
                    }
                });
            });
        },

        // Add loading state to buttons
        initButtonLoading: function () {
            const forms = document.querySelectorAll('form');
            if (!forms || forms.length === 0) return;
            
            forms.forEach(form => {
                form.addEventListener('submit', function () {
                    const submitBtn = this.querySelector('button[type="submit"]');
                    if (submitBtn && !submitBtn.disabled) {
                        submitBtn.disabled = true;
                        const originalText = submitBtn.innerHTML;
                        submitBtn.innerHTML = '<span class="spinner"></span> Processing...';
                        
                        // Re-enable after 3 seconds as fallback
                        setTimeout(() => {
                            submitBtn.disabled = false;
                            submitBtn.innerHTML = originalText;
                        }, 3000);
                    }
                });
            });
        },

        // Initialize tooltips (if using Bootstrap)
        initTooltips: function () {
            if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
                const tooltipTriggerList = [].slice.call(
                    document.querySelectorAll('[data-bs-toggle="tooltip"]')
                );
                tooltipTriggerList.map(function (tooltipTriggerEl) {
                    return new bootstrap.Tooltip(tooltipTriggerEl);
                });
            }
        },

        // Auto-hide alerts after 5 seconds
        initAutoHideAlerts: function () {
            document.querySelectorAll('.alert:not(.alert-permanent)').forEach(alert => {
                setTimeout(() => {
                    if (typeof bootstrap !== 'undefined' && bootstrap.Alert) {
                        const bsAlert = new bootstrap.Alert(alert);
                        bsAlert.close();
                    } else {
                        alert.style.opacity = '0';
                        setTimeout(() => alert.remove(), 300);
                    }
                }, 5000);
            });
        },

        // Confirm delete actions
        initDeleteConfirmation: function () {
            const deleteElements = document.querySelectorAll('[data-confirm-delete]');
            if (!deleteElements || deleteElements.length === 0) return;
            
            deleteElements.forEach(element => {
                if (!element) return;
                
                element.addEventListener('click', function (e) {
                    const message = this.getAttribute('data-confirm-delete') || 
                                  'Are you sure you want to delete this item?';
                    if (!confirm(message)) {
                        e.preventDefault();
                        return false;
                    }
                });
            });
        },

        // Add card hover effects
        initCardAnimations: function () {
            document.querySelectorAll('.card').forEach(card => {
                card.addEventListener('mouseenter', function () {
                    this.style.transform = 'translateY(-2px)';
                });
                card.addEventListener('mouseleave', function () {
                    this.style.transform = 'translateY(0)';
                });
            });
        },

        // Form validation helper
        validateForm: function (formElement) {
            const inputs = formElement.querySelectorAll('input[required], textarea[required], select[required]');
            let isValid = true;

            inputs.forEach(input => {
                if (!input.value.trim()) {
                    isValid = false;
                    input.classList.add('is-invalid');
                } else {
                    input.classList.remove('is-invalid');
                }
            });

            return isValid;
        },

        // Initialize all utilities
        init: function () {
            this.initSmoothScroll();
            this.initButtonLoading();
            this.initTooltips();
            this.initAutoHideAlerts();
            this.initDeleteConfirmation();
            this.initCardAnimations();
        }
    };

    // Initialize when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', function () {
            CRMUtils.init();
        });
    } else {
        CRMUtils.init();
    }

    // Expose CRMUtils globally for use in other scripts
    window.CRMUtils = CRMUtils;

})();