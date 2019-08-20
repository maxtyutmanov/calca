FROM nginx:latest

COPY /build /etc/nginx/html
COPY /nginx/nginx.conf /etc/nginx/conf.d/default.conf