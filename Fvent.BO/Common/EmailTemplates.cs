using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EmailTemplates
{
    public static readonly string EmailVerificationTemplate = $@"
    <!DOCTYPE html>
    <html lang='vi'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Xác Minh Email</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
            }}
            .container {{
                background-color: #ffffff;
                max-width: 600px;
                margin: 20px auto;
                padding: 20px;
                border-radius: 8px;
                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
            }}
            .header {{
                background-color: #4CAF50;
                color: #ffffff;
                padding: 10px 20px;
                border-top-left-radius: 8px;
                border-top-right-radius: 8px;
            }}
            .header h1 {{
                margin: 0;
                font-size: 24px;
            }}
            .content {{
                padding: 20px;
                color: #333333;
                line-height: 1.6;
            }}
            .content h2 {{
                color: #4CAF50;
            }}
            .btn {{
                display: inline-block;
                background-color: #4CAF50;
                color: white;
                padding: 10px 20px;
                text-align: center;
                text-decoration: none;
                border-radius: 5px;
                font-size: 16px;
                margin-top: 20px;
            }}
            .footer {{
                margin-top: 20px;
                font-size: 12px;
                color: #777777;
                text-align: center;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>Xác Nhận Địa Chỉ Email Của Bạn</h1>
            </div>
            <div class='content'>
                <h2>Chào mừng đến với Fvent!</h2>
                <p>Cảm ơn bạn đã đăng ký với chúng tôi. Để hoàn tất đăng ký, chúng tôi cần xác nhận địa chỉ email của bạn.</p>
                <p>Vui lòng nhấp vào nút dưới đây để xác minh email:</p>
                <a href='{{verificationLink}}' class='btn'>Xác Minh Email</a>
                <p>Nếu bạn không tạo tài khoản với Fvent, xin vui lòng bỏ qua email này.</p>
            </div>
            <div class='footer'>
                <p>Email này được gửi từ Fvent. Nếu bạn có bất kỳ thắc mắc nào, vui lòng <a href='mailto:thiendn.work@gmail.com'>liên hệ với chúng tôi</a>.</p>
            </div>
        </div>
    </body>
    </html>
    ";

    public static readonly string PasswordResetTemplate = $@"
        <!DOCTYPE html>
<html lang=""vi"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Đặt Lại Mật Khẩu</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}

        .container {{
            background-color: #ffffff;
            max-width: 600px;
            margin: 20px auto;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        }}

        .header {{
            background-color: #4CAF50;
            color: #ffffff;
            padding: 15px 20px;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
            text-align: center;
        }}

        .header h1 {{
            margin: 0;
            font-size: 24px;
        }}

        .content {{
            padding: 20px;
            color: #333333;
            line-height: 1.6;
            text-align: center;
        }}

        .content h2 {{
            color: #4CAF50;
        }}

        .btn {{
            display: inline-block;
            background-color: #4CAF50;
            color: white;
            padding: 12px 25px;
            text-align: center;
            text-decoration: none;
            border-radius: 5px;
            font-size: 16px;
            margin-top: 20px;
        }}

        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #777777;
            text-align: center;
        }}

        .footer p {{
            margin: 0;
        }}

        @media screen and (max-width: 600px) {{
            .container {{
                padding: 15px;
            }}

            .btn {{
                padding: 10px 20px;
                font-size: 14px;
            }}
        }}
    </style>
</head>

<body>
    <div class=""container"">
        <div class=""header"">
            <h1>Yêu Cầu Đặt Lại Mật Khẩu</h1>
        </div>
        <div class=""content"">
            <h2>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu của bạn.</h2>
            <p>Để đặt lại mật khẩu, vui lòng nhấp vào nút dưới đây:</p>
            <a href=""{{resetLink}}"" class=""btn"">Đặt Lại Mật Khẩu</a>
            <p>Nếu bạn không yêu cầu đặt lại mật khẩu, xin vui lòng bỏ qua email này.</p>
        </div>
        <div class=""footer"">
            <p>Email này được gửi từ Fvent. Nếu bạn có bất kỳ thắc mắc nào, vui lòng <a href=""mailto:thiendn.work@gmail.com"">liên hệ với chúng tôi</a>.</p>
        </div>
    </div>
</body>

</html>

";
}

