FROM nats:alpine

ADD nats-server.conf /nats-server.conf

ENTRYPOINT [ "nats-server" ]
CMD [ "--config", "/nats-server.conf"]
