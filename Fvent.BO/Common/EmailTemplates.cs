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
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
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
            padding: 10px 20px;
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
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Yêu Cầu Đặt Lại Mật Khẩu</h1>
        </div>
        <div class='content'>
            <h2>Xin chào {{userName}},</h2>
            <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu của bạn.</p>
            <p>Để đặt lại mật khẩu, vui lòng nhấn vào nút bên dưới:</p>
            <a href='{{resetLink}}' class='btn'>Đặt Lại Mật Khẩu</a>
            <p>Nếu bạn không yêu cầu đặt lại mật khẩu, xin vui lòng bỏ qua email này.</p>
            <p>Nếu có bất kỳ thắc mắc nào, vui lòng trả lời email này để được hỗ trợ kịp thời.</p>
        </div>
        <div class='footer'>
            <p>Email này được gửi từ Fvent. Nếu bạn có bất kỳ thắc mắc nào, vui lòng <a href='mailto:thiendn.work@gmail.com'>liên hệ với chúng tôi</a>.</p>
        </div>
    </div>
</body>
</html>
";


    public static readonly string ModeratorWelcomeTemplate = $@"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Chào Mừng Moderator</title>
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
            background-color: #007BFF;
            color: #ffffff;
            padding: 10px 20px;
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
        }}
        .content h2 {{
            color: #007BFF;
        }}
        .btn {{
            display: inline-block;
            background-color: #007BFF;
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
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Chào Mừng Moderator!</h1>
        </div>
        <div class='content'>
            <h2>Xin chào {{moderatorName}},</h2>
            <p>Chúng tôi rất vui mừng chào đón bạn đến với vai trò Moderator tại Fvent!</p>
            <p>Hãy bắt đầu bằng cách đăng nhập vào hệ thống để quản lý các sự kiện và hỗ trợ cộng đồng Fvent.</p>
            <p>Đừng ngần ngại liên hệ với đội ngũ hỗ trợ của chúng tôi nếu bạn cần thêm thông tin.</p>
            <p>Chúc bạn thành công trong vai trò mới!</p>
            <a href='{{loginLink}}' class='btn'>Đăng Nhập Ngay</a>
        </div>
        <div class='footer'>
            <p>Email này được gửi từ Fvent. Nếu bạn có bất kỳ thắc mắc nào, vui lòng <a href='mailto:thiendn.work@gmail.com'>liên hệ với chúng tôi</a>.</p>
        </div>
    </div>
</body>
</html>
";
    public static readonly string ApologyForEventCancellationTemplate = $@"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Xin Lỗi Vì Hủy Sự Kiện</title>
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
            background-color: #FF5733;
            color: #ffffff;
            padding: 10px 20px;
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
        }}
        .content h2 {{
            color: #FF5733;
        }}
        .btn {{
            display: inline-block;
            background-color: #FF5733;
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
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Xin Lỗi Vì Hủy Sự Kiện</h1>
        </div>
        <div class='content'>
            <h2>Xin chào {{userName}},</h2>
            <p>Chúng tôi rất tiếc phải thông báo rằng sự kiện <strong>{{eventName}}</strong>, dự kiến diễn ra vào ngày <strong>{{eventStartDate}}</strong>, đã bị hủy.</p>
            <p>Chúng tôi hiểu rằng quyết định này có thể đã gây ra sự bất tiện cho bạn và rất mong nhận được sự thông cảm của bạn. Quyết định hủy sự kiện được đưa ra dựa trên các yếu tố không mong muốn, và chúng tôi đang cố gắng hết sức để tránh những trường hợp tương tự trong tương lai.</p>
            <p>Nếu bạn cần hỗ trợ thêm hoặc có bất kỳ thắc mắc nào, xin vui lòng liên hệ với chúng tôi qua email hoặc nhấn vào nút bên dưới để được hỗ trợ ngay lập tức.</p>
            <p>Một lần nữa, chúng tôi chân thành xin lỗi vì sự bất tiện này và cảm ơn sự ủng hộ của bạn.</p>
            <a href='{{supportLink}}' class='btn'>Liên Hệ Hỗ Trợ</a>
        </div>
        <div class='footer'>
            <p>Email này được gửi từ Fvent. Nếu bạn có bất kỳ thắc mắc nào, vui lòng <a href='mailto:thiendn.work@gmail.com'>liên hệ với chúng tôi</a>.</p>
        </div>
    </div>
</body>
</html>
";
    public static readonly string PermanentAccountDeletionTemplate = $@"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Thông Báo Về Việc Khóa Tài Khoản</title>
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
            background-color: #FF5733;
            color: #ffffff;
            padding: 10px 20px;
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
        }}
        .content h2 {{
            color: #FF5733;
        }}
        .btn {{
            display: inline-block;
            background-color: #FF5733;
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
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Thông Báo Về Việc Khóa Tài Khoản</h1>
        </div>
        <div class='content'>
            <h2>Xin chào {{userName}},</h2>
            <p>Chúng tôi rất tiếc phải thông báo rằng tài khoản của bạn đã bị khóa vĩnh viễn do vi phạm các điều khoản và chính sách của chúng tôi.</p>
            <p>Quyết định này không hề dễ dàng và được đưa ra sau khi cân nhắc kỹ lưỡng. Chúng tôi hy vọng bạn sẽ xem xét các điều khoản sử dụng để tránh những vi phạm tương tự trong tương lai.</p>
            <p>Nếu bạn có bất kỳ câu hỏi nào hoặc cần giải thích thêm về quyết định này, vui lòng trả lời email này. Chúng tôi sẽ sẵn sàng hỗ trợ bạn.</p>
            <p>Cảm ơn bạn đã sử dụng nền tảng của chúng tôi.</p>
        </div>
        <div class='footer'>
            <p>Email này được gửi từ Fvent. Nếu bạn có bất kỳ thắc mắc nào, vui lòng <a href='mailto:thiendn.work@gmail.com'>liên hệ với chúng tôi</a>.</p>
        </div>
    </div>
</body>
</html>
";
    public static readonly string UnbanUserNotificationTemplate = $@"
<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Thông Báo Gỡ Khóa Tài Khoản</title>
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
            background-color: #28a745;
            color: #ffffff;
            padding: 10px 20px;
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
        }}
        .content h2 {{
            color: #28a745;
        }}
        .btn {{
            display: inline-block;
            background-color: #28a745;
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
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Thông Báo Gỡ Khóa Tài Khoản</h1>
        </div>
        <div class='content'>
            <h2>Xin chào {{userName}},</h2>
            <p>Chúng tôi rất vui mừng thông báo rằng tài khoản của bạn đã được gỡ khóa và bạn có thể tiếp tục sử dụng dịch vụ của chúng tôi.</p>
            <p>Chúng tôi hy vọng bạn sẽ tuân thủ các quy định và điều khoản của nền tảng để mang lại trải nghiệm tốt đẹp nhất cho tất cả người dùng.</p>
            <p>Bạn có thể đăng nhập lại tài khoản của mình bằng cách nhấn vào nút bên dưới:</p>
            <a href='{{loginLink}}' class='btn'>Đăng Nhập Ngay</a>
            <p>Nếu bạn có bất kỳ thắc mắc nào hoặc cần hỗ trợ, vui lòng trả lời email này. Chúng tôi sẽ sẵn sàng hỗ trợ bạn.</p>
            <p>Cảm ơn bạn đã đồng hành cùng chúng tôi!</p>
        </div>
        <div class='footer'>
            <p>Email này được gửi từ Fvent. Nếu bạn có bất kỳ thắc mắc nào, vui lòng <a href='mailto:thiendn.work@gmail.com'>liên hệ với chúng tôi</a>.</p>
        </div>
    </div>
</body>
</html>
";


}

