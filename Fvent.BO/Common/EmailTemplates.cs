using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EmailTemplates
{
    public static readonly string EmailVerificationTemplate = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Email Verification</title>
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
                <h1>Confirm Your Email Address</h1>
            </div>
            <div class='content'>
                <h2>Welcome to Fvent!</h2>
                <p>Thank you for signing up with us. To complete your registration, we just need to confirm your email address.</p>
                <p>Please click the button below to verify your email:</p>
                <a href='{{verificationLink}}' class='btn'>Verify Email</a>
                <p>If you didn't create an account with Fvent, please disregard this email.</p>
            </div>
            <div class='footer'>
                <p>This email was sent by Fvent. If you have any questions, feel free to <a href='mailto:support@fvent.com'>contact us</a>.</p>
            </div>
        </div>
    </body>
    </html>
    ";
    public static readonly string PasswordResetTemplate = $@"
        <!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Password Reset</title>
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
            <h1>Password Reset Request</h1>
        </div>
        <div class=""content"">
            <h2>We received a request to reset your password.</h2>
            <p>To reset your password, please click the button below:</p>
            <a href=""{{resetLink}}"" class=""btn"">Reset Password</a>
            <p>If you didn't request this, please disregard this email.</p>
        </div>
        <div class=""footer"">
            <p>This email was sent by Fvent. If you have any questions, feel free to <a href=""mailto:support@fvent.com"">contact us</a>.</p>
        </div>
    </div>
</body>

</html>



        ";
}

