using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using sight.Models;

namespace sight.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private sightEntities db = new sightEntities();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //هذا الكود يتحقق إذا كان المستخدم مسجل دخوله، إذا كان كذلك، فإنه سيتم إعادة توجيه المستخدم إلى الصفحة الرئيسية "Home". يستخدم هذا الكود بشكل رئيسي في صفحة تسجيل الدخول أو التسجيل، لأنه يمنع المستخدم الذي سجل دخوله أو الذي تم تسجيله مسبقًا من الوصول إلى صفحة التسجيل أو تسجيل الدخول مرة أخرى. يتم تحديد ذلك عادة لتجنب إعادة تسجيل الدخول مرة أخرى واستخدام ذلك في الغالب للأمان.
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:

                    var user = await UserManager.FindByEmailAsync(model.Email);
                    if (await UserManager.IsInRoleAsync(user.Id, "Admin"))
                    {
                        return RedirectToAction("Dashboard", "AdminDashboard");
                    }
                    else if (await UserManager.IsInRoleAsync(user.Id, "Photographer"))
                    {
                        return RedirectToAction("Edit", "photographersProfile");

                    }
                    return RedirectToLocal(returnUrl);

    
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        // GET: comments/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.id = id;

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            comment comment = db.comments.Find(id);
            //if (comment == null)
            //{
            //    return HttpNotFound();
            //}
            return View(comment);
        }

        // POST: comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            comment comment = db.comments.Find(id);
            db.comments.Remove(comment);
            db.SaveChanges();
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            if (UserManager.IsInRole(currentUser.Id, "Client"))
            {
                return RedirectToAction("Edit", "Clients");
            }
            else if (UserManager.IsInRole(currentUser.Id, "Photographer"))
            {
                return RedirectToAction("Edit", "photographersProfile");
            }
            else
            {
                // Return to a default action if user role is not found
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DeleteAccount()
        {
            // Get the current user
            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            // Change the email in AspNetUsers table
            var newEmail = "deleteaccount." + currentUser.Email;
            var newUser = "deleteaccount." + currentUser.UserName;

            currentUser.Email = newEmail;
            currentUser.UserName = newUser;

            UserManager.Update(currentUser);

            // Change the state and is_hidden in photographers table
            var photographer = db.photographers.Where(p => p.user_id == currentUser.Id).FirstOrDefault();
            if (photographer != null)
            {
                photographer.state = "deleted";
                photographer.is_hidden = true;
                photographer.profilePhoto = "photographerProfile.png";
                photographer.coverPhoto = "newPhotographer.jpg";
                photographer.FullName = "Sight Photographer";
                db.Entry(photographer).State = EntityState.Modified;
                db.SaveChanges();
            }
            var subscription = db.Subscriptions.Where(p => p.PhotographerId == photographer.id).FirstOrDefault();
            if (subscription != null)
            {
                subscription.startDate = DateTime.Now.AddDays(-1); 
                subscription.endDate = DateTime.Now.AddDays(-1);
                db.Entry(subscription).State = EntityState.Modified;
                db.SaveChanges();
            }

            // Sign out the user and redirect to home page
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            TempData["DeletMessage"] = "The account has been deleted successfully.";
            return RedirectToAction("Login", "Account");

        }

        public ActionResult DeleteAccountClient()
        {
            // Get the current user
            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            // Change the email in AspNetUsers table
            var newEmail = "deleteaccount." + currentUser.Email;
            var newUser = "deleteaccount." + currentUser.UserName;

            currentUser.Email = newEmail;
            currentUser.UserName = newUser;

            UserManager.Update(currentUser);

            // Change the state and is_hidden in photographers table
            var client = db.clients.Where(p => p.user_id == currentUser.Id).FirstOrDefault();
            if (client != null)
            {
                client.photo = "cliantProfile.png";
                client.fullName = "Sight User";
                client.state = "deleted";
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
            }

            // Sign out the user and redirect to home page
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            TempData["DeletMessage"] = "The account has been deleted successfully.";
            return RedirectToAction("Login", "Account");

        }

        public ActionResult PhotographerProfileForm()
        {
            var photographer = User.Identity.GetUserId();
            int photographerId = db.photographers.FirstOrDefault(a => a.user_id == photographer).id;
            photographer photographers = db.photographers.Find(photographerId);


            return View(photographers);
        }



        //public ActionResult butPhotographer([Bind(Include = "user_id,FullName,profilePhoto,coverPhoto,bio,age,instagram,facebook,twitter,linkedin")] photographer photographers, HttpPostedFileBase image, HttpPostedFileBase cv)
        //{
        //    //photographers.user_id = Session["photographerID"].ToString();
        //    //db.photographers.Add(photographers);

        //    db.SaveChanges();
        //    return View();

        //}
      
        public ActionResult butPhotographer([Bind(Include = "id,user_id,FullName,subscription_type,type_of_photography,areas,profilePhoto,coverPhoto,bio,accept,is_hidden,created_at")] photographer photographer, HttpPostedFileBase image, HttpPostedFileBase cv)
        {

            if (ModelState.IsValid)
            {
                db.Entry(photographer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", photographer.user_id);
            return View(photographer);
        }







        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            //هذا الكود يتحقق إذا كان المستخدم مسجل دخوله، إذا كان كذلك، فإنه سيتم إعادة توجيه المستخدم إلى الصفحة الرئيسية "Home". يستخدم هذا الكود بشكل رئيسي في صفحة تسجيل الدخول أو التسجيل، لأنه يمنع المستخدم الذي سجل دخوله أو الذي تم تسجيله مسبقًا من الوصول إلى صفحة التسجيل أو تسجيل الدخول مرة أخرى. يتم تحديد ذلك عادة لتجنب إعادة تسجيل الدخول مرة أخرى واستخدام ذلك في الغالب للأمان.

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string Account)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    //Session["photographerID"] = user.Id;
                    if (Account == "Photographer")
                    {
                        // تعيين الدور للمستخدم
                        await UserManager.AddToRoleAsync(user.Id, "Photographer");
                        //  Photographer
                        photographer photographers = new photographer();
                        photographers.user_id = user.Id;
                        photographers.profilePhoto = "photographerProfile.png";
                        photographers.coverPhoto = "newPhotographer.jpg";
                        photographers.FullName = "Sight Photographer";
                        photographers.accept = false;
                        photographers.is_hidden = true;
                        photographers.facebook = "//facebook.com/#";
                        photographers.instagram = "//instagram.com/#";
                        photographers.twitter = "//twitter.com/#";
                        photographers.linkedin = "//linkedin.com/in/#";
                        photographers.created_at = DateTime.Now; // تعيين تاريخ التسجيل الحالي

                        Subscription subscription = new Subscription();
                        subscription.PhotographerId = photographers.id;
                        subscription.Price = 0;

                        db.photographers.Add(photographers);
                        db.Subscriptions.Add(subscription);
                        db.SaveChanges();
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // تفعيل الـ Alert Message
                        TempData["alertMessage"] = "Your account is created successfully. Your account will be reviewed by the site administration, and you will have 7 days free trial period from the moment your account is approved. After that, you need to choose the paid subscription that suits you to continue appearing on the site.";

                        var userId = User.Identity.GetUserId();
                        return RedirectToAction("Edit", "photographersProfile", new { id = userId });
                    }
                    else if (Account == "Client")
                    {
                        // تعيين الدور للمستخدم
                        await UserManager.AddToRoleAsync(user.Id, "Client");
                        //  Client
                        client clients = new client();
                        clients.user_id = user.Id;
                        clients.photo = "cliantProfile.png";
                        clients.fullName = "Sight User";
                        clients.created_at = DateTime.Now; // تعيين تاريخ التسجيل الحالي
                        db.clients.Add(clients);
                        db.SaveChanges();
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        var userId = User.Identity.GetUserId();
                        return RedirectToAction("Edit", "clients", new { id = userId });
                        //return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // 
                        ModelState.AddModelError("", "Please choose account type.");
                        return View(model);
                    }
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public async Task<ActionResult> BlockUser(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Check if the user is already blocked
                if (!await userManager.IsInRoleAsync(id, "block"))
                {
                    // Remove all existing roles for the user
                    var userRoles = await userManager.GetRolesAsync(id);
                    await userManager.RemoveFromRolesAsync(id, userRoles.ToArray());

                    // Add the block role to the user
                    await userManager.AddToRoleAsync(id, "block");

                    // Save the changes
                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        // Update the photographer's state to "block"
                        photographer photographers = new photographer();
                        var photographer = db.photographers.FirstOrDefault(p => p.user_id == id);
                        if (photographer != null)
                        {
                            photographer.state = "block";
                            db.Entry(photographer).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        return RedirectToAction("blackList", "AspNetUsers");
                    }
                }
            }

            // Redirect the user to an error page if the user is not found or the update fails
            return RedirectToAction("state", "photographers");
        }


        public async Task<ActionResult> BlockUserClient(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Check if the user is already blocked
                if (!await userManager.IsInRoleAsync(id, "block"))
                {
                    // Remove all existing roles for the user
                    var userRoles = await userManager.GetRolesAsync(id);
                    await userManager.RemoveFromRolesAsync(id, userRoles.ToArray());

                    // Add the block role to the user
                    await userManager.AddToRoleAsync(id, "block");

                    // Save the changes
                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        // Update the photographer's state to "block"
                        client clients = new client();
                        var client = db.clients.FirstOrDefault(p => p.user_id == id);
                        if (client != null)
                        {
                            client.state = "block";
                            db.Entry(client).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        return RedirectToAction("blackListClient", "AspNetUsers");
                    }
                }
            }

            // Redirect the user to an error page if the user is not found or the update fails
            return RedirectToAction("state", "clients");
        }



        public async Task<ActionResult> UnblockUserClient(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Check if the user is blocked
                if (await userManager.IsInRoleAsync(id, "block"))
                {
                    // Remove all existing roles for the user
                    var userRoles = await userManager.GetRolesAsync(id);
                    await userManager.RemoveFromRolesAsync(id, userRoles.ToArray());

                    // Add the block role to the user
                    await userManager.AddToRoleAsync(id, "Client");

                    // Save the changes
                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        // Update the client's state to "unblock"
                        var client = db.clients.FirstOrDefault(p => p.user_id == id);
                        if (client != null)
                        {
                            client.state = "unblock";
                            db.Entry(client).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        return RedirectToAction("blackListClient", "AspNetUsers");
                    }
                }
            }

            // Redirect the user to an error page if the user is not found or the update fails
            return RedirectToAction("blackListClient", "AspNetUsers");
        }




        public async Task<ActionResult> UnblockUser(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Check if the user is blocked
                if (await userManager.IsInRoleAsync(id, "block"))
                {
                    // Remove all existing roles for the user
                    var userRoles = await userManager.GetRolesAsync(id);
                    await userManager.RemoveFromRolesAsync(id, userRoles.ToArray());

                    // Add the block role to the user
                    await userManager.AddToRoleAsync(id, "Photographer");

                    // Save the changes
                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        // Update the photographer's state to "unblock"
                        photographer photographers = new photographer();
                        var photographer = db.photographers.FirstOrDefault(p => p.user_id == id);
                        if (photographer != null)
                        {
                            photographer.state = "unblock";
                            db.Entry(photographer).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        return RedirectToAction("blackList", "AspNetUsers");
                    }
                }
            }

            // Redirect the user to an error page if the user is not found or the update fails
            return RedirectToAction("blackList", "AspNetUsers");
        }








        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}