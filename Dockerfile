FROM nats:latest

ADD ./nats.conf /var/opt/nats.conf

ENTRYPOINT [ "nats-server" ]
CMD [ "-c", "/var/opt/nats.conf" ]
