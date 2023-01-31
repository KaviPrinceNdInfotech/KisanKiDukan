/// <reference path="Library/bootstrapvalidator.min.js"/>
    /// <reference path="Library/bootstrap.min.js"/>
/// <reference path="Library/jquery-1.7.1.min.js" />
    
        $(document).ready(function () {
            $('#contact_form').bootstrapValidator({
                // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    FullName: {
                        validators: {
                            stringLength: {
                                min: 4,
                            },
                            notEmpty: {
                                message: 'Please enter your name'
                            }
                        }
                    },
                    
                    Email_Id: {
                        validators: {
                            notEmpty: {
                                message: 'Please enter your email address'
                            },
                            emailAddress: {
                                message: 'Please enter a valid email address'
                            }
                        }
                    },
                    Phone: {                       
                        validators: {
                            stringLength: {
                                min: 10,
                                max: 10,
                                message: 'Please enter 10 digit and no more than 10 digit of no '
                            },
                            notEmpty: {
                                message: 'Mobile number required'
                            }, // notEmpty
                            regexp: {
                                regexp: /^[1-9][0-9]{0,10}$/,
                                message: "Invalid Mobile Number"
                            }
                        }
                        // validators
                    },  // mobilenum 
                    Address: {
                        validators: {
                            stringLength: {
                                min: 8,
                            },
                            notEmpty: {
                                message: 'Please enter your full address'
                            }
                        }
                    },
                    
                    Password: {
                        validators: {
                            
                            stringLength: {
                                min: 5,
                                max: 20,
                                message: 'Please enter at least 5 characters and no more than 20'
                            },
                            notEmpty: {
                                message: 'Please enter a password'
                            }
                        }
                    },
                    confirmpassword: {
                        validators: {
                            identical: {
                                field: 'Password',
                                message: 'Password and Confirm Password not match'
                            },
                            notEmpty: {
                                message: 'Please enter a Confirm password'
                            }

                        }
                    },
                 
                }
            })
                .on('success.form.bv', function (e) {
                    $('#success_message').slideDown({ opacity: "show" }, "slow") // Do something ...
                    $('#contact_form').data('bootstrapValidator').resetForm();

                    // Prevent form submission
                    e.preventDefault();

                    // Get the form instance
                    var $form = $(e.target);

                    // Get the BootstrapValidator instance
                    var bv = $form.data('bootstrapValidator');

                    // Use Ajax to submit form data
                    $.post($form.attr('action'), $form.serialize(), function (result) {
                        console.log(result);
                    }, 'json');
                });
        });
